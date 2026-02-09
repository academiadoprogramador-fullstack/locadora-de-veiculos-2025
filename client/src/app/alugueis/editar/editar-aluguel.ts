import { format, parse } from 'date-fns';
import { NgxMaskDirective } from 'ngx-mask';
import { filter, map, Observer, shareReplay, switchMap, take, tap } from 'rxjs';

import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { SelecionarCondutoresModel } from '../../condutores/condutor.models';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { SelecionarTaxasModel } from '../../taxas/taxa.models';
import { SelecionarVeiculosModel } from '../../veiculos/veiculo.models';
import {
    DetalhesAluguelModel, EditarAluguelModel, SimularAberturaAluguelResponseModel, StatusAluguel,
    TipoPlanoCobranca
} from '../aluguel.model';
import { AluguelService } from '../aluguel.service';

@Component({
  selector: 'app-editar-aluguel',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
    NgxMaskDirective,
    AsyncPipe,
    CurrencyPipe,
  ],
  templateUrl: './editar-aluguel.html',
})
export class EditarAluguel {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly aluguelService = inject(AluguelService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly tiposPlano = Object.values(TipoPlanoCobranca);

  protected readonly condutores$ = this.route.data.pipe(
    filter((d) => d['condutores']),
    map((d) => d['condutores'] as SelecionarCondutoresModel[]),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected readonly veiculos$ = this.route.data.pipe(
    filter((d) => d['veiculos']),
    map((d) => d['veiculos'] as SelecionarVeiculosModel[]),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected readonly taxas$ = this.route.data.pipe(
    filter((d) => d['taxas']),
    map((d) => d['taxas'] as SelecionarTaxasModel[]),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected aluguelForm: FormGroup = this.formBuilder.group({
    condutorId: [null, [Validators.required]],
    veiculoId: [null, [Validators.required]],
    tipoPlano: [TipoPlanoCobranca.Diario, [Validators.required]],
    inicioEmUtc: [format(new Date(), 'dd/MM/yyyy'), [Validators.required]],
    devolucaoPrevistaEmUtc: [format(new Date(), 'dd/MM/yyyy'), [Validators.required]],
    taxasSelecionadasIds: [[]],
  });

  get condutorId() {
    return this.aluguelForm.get('condutorId');
  }

  get veiculoId() {
    return this.aluguelForm.get('veiculoId');
  }

  get tipoPlano() {
    return this.aluguelForm.get('tipoPlano');
  }

  get inicioEmUtc() {
    return this.aluguelForm.get('inicioEmUtc');
  }

  get devolucaoPrevistaEmUtc() {
    return this.aluguelForm.get('devolucaoPrevistaEmUtc');
  }

  get taxasSelecionadasIds() {
    return this.aluguelForm.get('taxasSelecionadasIds');
  }

  protected readonly aluguel$ = this.route.data.pipe(
    filter((d) => d['aluguel']),
    map((d) => d['aluguel'] as DetalhesAluguelModel),
    tap((aluguel) => {
      this.aluguelForm.patchValue(
        {
          condutorId: aluguel.condutor.id,
          veiculoId: aluguel.veiculo.id,
          tipoPlano: aluguel.tipoPlano,
          inicioEmUtc: format(new Date(aluguel.inicioEmUtc), 'dd/MM/yyyy'),
          devolucaoPrevistaEmUtc: format(new Date(aluguel.devolucaoPrevistaEmUtc), 'dd/MM/yyyy'),
          taxasSelecionadasIds: aluguel.taxasSelecionadas?.map((t) => t.id) ?? [],
        },
        { emitEvent: false },
      );

      if (aluguel.status !== StatusAluguel.Simulacao) {
        this.notificacaoService.erro('Apenas aluguéis em simulação podem ser editados.');

        this.router.navigate(['/alugueis']);
      }
    }),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.aluguelForm.invalid) return;

    const raw = this.aluguelForm.getRawValue();

    const model: EditarAluguelModel = {
      condutorId: raw.condutorId,
      veiculoId: raw.veiculoId,
      tipoPlano: raw.tipoPlano,
      inicioEmUtc: parse(raw.inicioEmUtc, 'dd/MM/yyyy', new Date()),
      devolucaoPrevistaEmUtc: parse(raw.devolucaoPrevistaEmUtc, 'dd/MM/yyyy', new Date()),
      taxasSelecionadasIds: Array.isArray(raw.taxasSelecionadasIds)
        ? [...raw.taxasSelecionadasIds]
        : [],
    };

    const edicaoObserver: Observer<SimularAberturaAluguelResponseModel> = {
      next: (res) => {
        this.notificacaoService.sucesso('Aluguel editado com sucesso!');

        this.router.navigate(['/alugueis', 'abrir', res.id], {
          state: { aluguelSimulado: res },
        });
      },
      error: (err) => this.notificacaoService.erro(err),
      complete: () => {},
    };

    this.aluguel$
      .pipe(
        take(1),
        switchMap((aluguel) => {
          if (aluguel.status !== StatusAluguel.Simulacao) {
            this.notificacaoService.erro('Apenas aluguéis em simulação podem ser editados.');
            throw new Error('Aluguel não está em simulação.');
          }

          return this.aluguelService.editar(aluguel.id, model);
        }),
      )
      .subscribe(edicaoObserver);
  }
}
