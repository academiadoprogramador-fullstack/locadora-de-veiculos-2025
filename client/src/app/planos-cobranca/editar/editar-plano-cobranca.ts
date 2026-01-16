import { NgxMaskDirective } from 'ngx-mask';
import { filter, map, Observer, shareReplay, switchMap, take, tap } from 'rxjs';

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

import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
    DetalhesPlanoCobrancaModel, EditarPlanoCobrancaModel, EditarPlanoCobrancaResponseModel
} from '../plano-cobranca.models';
import { PlanoCobrancaService } from '../plano-cobranca.service';

@Component({
  selector: 'app-editar-plano-cobranca',
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
  templateUrl: './editar-plano-cobranca.html',
})
export class EditarPlanoCobranca {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly planoCobrancaService = inject(PlanoCobrancaService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected planoCobrancaForm: FormGroup = this.formBuilder.group({
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

  get precosPlanoDiario() {
    return this.planoCobrancaForm.get('precosPlanoDiario');
  }

  get precosPlanoControlado() {
    return this.planoCobrancaForm.get('precosPlanoControlado');
  }

  get precosPlanoLivre() {
    return this.planoCobrancaForm.get('precosPlanoLivre');
  }

  protected readonly planoCobranca$ = this.route.data.pipe(
    filter((data) => data['planoCobranca']),
    map((data) => data['planoCobranca'] as DetalhesPlanoCobrancaModel),
    tap((planoCobranca) => this.planoCobrancaForm.patchValue(planoCobranca)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.planoCobrancaForm.invalid) return;

    const planoCobrancaModel: EditarPlanoCobrancaModel = this.planoCobrancaForm.value;

    const edicaoObserver: Observer<EditarPlanoCobrancaResponseModel> = {
      next: () => this.notificacaoService.sucesso(`O plano de cobrança foi editado com sucesso!`),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/planos-cobranca']),
    };

    this.planoCobranca$
      .pipe(
        take(1),
        switchMap((planoCobranca) =>
          this.planoCobrancaService.editar(planoCobranca.id, planoCobrancaModel),
        ),
      )
      .subscribe(edicaoObserver);
  }
}
