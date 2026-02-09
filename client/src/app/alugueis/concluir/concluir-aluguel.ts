import { NgxMaskDirective } from 'ngx-mask';
import { filter, map, Observer, shareReplay, switchMap, take, tap } from 'rxjs';

import { AsyncPipe, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
    ConcluirAluguelModel, DetalhesAluguelModel, MarcadorCombustivel, SimularConclusaoAluguelModel,
    SimularConclusaoAluguelResponseModel, TaxaDto
} from '../aluguel.model';
import { AluguelService } from '../aluguel.service';

@Component({
  selector: 'app-concluir-aluguel',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
    NgxMaskDirective,
    AsyncPipe,
    CurrencyPipe,
    DatePipe,
  ],
  templateUrl: './concluir-aluguel.html',
})
export class ConcluirAluguel {
  private readonly formBuilder = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly aluguelService = inject(AluguelService);
  private readonly notificacaoService = inject(NotificacaoService);

  protected readonly marcadorCombustivelEnum = Object.values(MarcadorCombustivel);

  protected readonly aluguel$ = this.route.data.pipe(
    filter((d) => d['aluguel']),
    map((d) => d['aluguel'] as DetalhesAluguelModel),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected readonly taxas$ = this.route.data.pipe(
    filter((d) => d['taxas']),
    map((d) => d['taxas'] as TaxaDto[]),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  // resultado da simulação (fica visível no template)
  protected readonly simulacao = signal<SimularConclusaoAluguelResponseModel | null>(null);

  protected conclusaoForm: FormGroup = this.formBuilder.group({
    quilometragemPercorrida: [null, [Validators.required]],
    marcadorCombustivel: [MarcadorCombustivel.Completo, [Validators.required]],
    taxasAdicionaisIds: [[]],
  });

  get quilometragemPercorrida() {
    return this.conclusaoForm.get('quilometragemPercorrida');
  }

  get marcadorCombustivel() {
    return this.conclusaoForm.get('marcadorCombustivel');
  }

  get taxasAdicionaisIds() {
    return this.conclusaoForm.get('taxasAdicionaisIds');
  }

  /** SIMULAR conclusão: calcula valores e exibe no card */
  public simular() {
    if (this.conclusaoForm.invalid) return;

    const raw = this.conclusaoForm.getRawValue();

    const model: SimularConclusaoAluguelModel = {
      quilometragemPercorrida: Number(raw.quilometragemPercorrida),
      marcadorCombustivel: raw.marcadorCombustivel,
      taxasAdicionaisIds: Array.isArray(raw.taxasAdicionaisIds) ? [...raw.taxasAdicionaisIds] : [],
    };

    const simObserver: Observer<SimularConclusaoAluguelResponseModel> = {
      next: (res) => {
        this.simulacao.set(res);
        this.notificacaoService.sucesso(`Simulação gerada! Total: R$ ${res.valorTotal.toFixed(2)}`);
      },
      error: (err) => this.notificacaoService.erro(err),
      complete: () => {},
    };

    this.aluguel$
      .pipe(
        take(1),
        switchMap((aluguel) => this.aluguelService.simularConclusao(aluguel.id, model)),
      )
      .subscribe(simObserver);
  }

  /** CONFIRMAR conclusão: exige que simulação exista para evitar concluir "no escuro" */
  public concluir() {
    if (this.conclusaoForm.invalid) return;

    const sim = this.simulacao();
    if (!sim) {
      this.notificacaoService.erro('Simule a conclusão antes de confirmar.');
      return;
    }

    const raw = this.conclusaoForm.getRawValue();

    const model: ConcluirAluguelModel = {
      quilometragemPercorrida: Number(raw.quilometragemPercorrida),
      marcadorCombustivel: raw.marcadorCombustivel,
      taxasAdicionaisIds: Array.isArray(raw.taxasAdicionaisIds) ? [...raw.taxasAdicionaisIds] : [],
    };

    const concluirObserver: Observer<unknown> = {
      next: () => this.notificacaoService.sucesso('Aluguel concluído com sucesso!'),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/alugueis']),
    };

    this.aluguel$
      .pipe(
        take(1),
        switchMap((aluguel) => this.aluguelService.concluir(aluguel.id, model)),
      )
      .subscribe(concluirObserver);
  }

  /* se mudar algum campo, invalidar simulação (pra evitar confirmar com valores antigos) */
  constructor() {
    this.conclusaoForm.valueChanges
      .pipe(
        tap(() => {
          if (this.simulacao()) this.simulacao.set(null);
        }),
      )
      .subscribe();
  }
}
