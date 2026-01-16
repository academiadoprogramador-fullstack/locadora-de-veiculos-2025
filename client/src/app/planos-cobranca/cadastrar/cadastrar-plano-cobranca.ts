import { NgxMaskDirective } from 'ngx-mask';
import { filter, map, Observer, shareReplay } from 'rxjs';

import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { SelecionarGruposVeiculosModel } from '../../grupos-veiculos/grupo-veiculos.models';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
    CadastrarPlanoCobrancaModel, CadastrarPlanoCobrancaResponseModel
} from '../plano-cobranca.models';
import { PlanoCobrancaService } from '../plano-cobranca.service';

@Component({
  selector: 'app-cadastrar-plano-cobranca',
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
  ],
  templateUrl: './cadastrar-plano-cobranca.html',
})
export class CadastrarPlanoCobranca {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly planoCobrancaService = inject(PlanoCobrancaService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly gruposVeiculos$ = this.route.data.pipe(
    filter((data) => data['gruposVeiculos']),
    map((data) => data['gruposVeiculos'] as SelecionarGruposVeiculosModel[]),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected planoCobrancaForm: FormGroup = this.formBuilder.group({
    grupoVeiculosId: [null, [Validators.required]],

    precosPlanoDiario: this.formBuilder.group({
      precoDiarioPlanoDiario: [0, [Validators.required, Validators.min(0)]],
      precoQuilometroPlanoDiario: [0, [Validators.required, Validators.min(0)]],
    }),

    precosPlanoControlado: this.formBuilder.group({
      precoDiarioPlanoControlado: [0, [Validators.required, Validators.min(0)]],
      quilometrosDisponiveisPlanoControlado: [0, [Validators.required, Validators.min(0)]],
      precoQuilometroExtrapoladoPlanoControlado: [0, [Validators.required, Validators.min(0)]],
    }),

    precosPlanoLivre: this.formBuilder.group({
      precoDiarioPlanoLivre: [0, [Validators.required, Validators.min(0)]],
    }),
  });

  get grupoVeiculosId() {
    return this.planoCobrancaForm.get('grupoVeiculosId');
  }

  get precosPlanoDiario() {
    return this.planoCobrancaForm.get('precosPlanoDiario');
  }

  get precosPlanoControlado() {
    return this.planoCobrancaForm.get('precosPlanoControlado');
  }

  get precosPlanoLivre() {
    return this.planoCobrancaForm.get('precosPlanoLivre');
  }

  public cadastrar() {
    if (this.planoCobrancaForm.invalid) return;

    const planoCobrancaModel: CadastrarPlanoCobrancaModel = this.planoCobrancaForm.value;

    const cadastroObserver: Observer<CadastrarPlanoCobrancaResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(`O plano de cobrança foi cadastrado com sucesso!`),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/planos-cobranca']),
    };

    this.planoCobrancaService.cadastrar(planoCobrancaModel).subscribe(cadastroObserver);
  }
}
