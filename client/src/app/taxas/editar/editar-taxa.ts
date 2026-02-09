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
  DetalhesTaxaModel,
  EditarTaxaModel,
  EditarTaxaResponseModel,
  TipoCobrancaTaxa,
} from '../taxa.models';
import { TaxaService } from '../taxa.service';

@Component({
  selector: 'app-editar-taxa',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
    AsyncPipe,
  ],
  templateUrl: './editar-taxa.html',
})
export class EditarTaxa {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly taxaService = inject(TaxaService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly tiposCobranca = Object.values(TipoCobrancaTaxa);

  protected taxaForm: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required, Validators.minLength(3)]],
    valor: [0, [Validators.required, Validators.min(0.01)]],
    tipoCobranca: [TipoCobrancaTaxa.Fixa, [Validators.required]],
  });

  get nome() {
    return this.taxaForm.get('nome');
  }

  get valor() {
    return this.taxaForm.get('valor');
  }

  get tipoCobranca() {
    return this.taxaForm.get('tipoCobranca');
  }

  protected readonly taxa$ = this.route.data.pipe(
    filter((data) => data['taxa']),
    map((data) => data['taxa'] as DetalhesTaxaModel),
    tap((taxa) => this.taxaForm.patchValue({ ...taxa }, { emitEvent: false })),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.taxaForm.invalid) return;

    const model: EditarTaxaModel = this.taxaForm.value;

    const edicaoObserver: Observer<EditarTaxaResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(`A taxa "${model.nome}" foi editada com sucesso!`),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/taxas']),
    };

    this.taxa$
      .pipe(
        take(1),
        switchMap((taxaOriginal) => this.taxaService.editar(taxaOriginal.id, model)),
      )
      .subscribe(edicaoObserver);
  }
}
