import { Observer } from 'rxjs';

import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router, RouterLink } from '@angular/router';

import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { CadastrarTaxaModel, CadastrarTaxaResponseModel, TipoCobrancaTaxa } from '../taxa.models';
import { TaxaService } from '../taxa.service';

@Component({
  selector: 'app-cadastrar-taxa',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './cadastrar-taxa.html',
})
export class CadastrarTaxa {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
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

  public cadastrar() {
    if (this.taxaForm.invalid) return;

    const model: CadastrarTaxaModel = this.taxaForm.value;

    const cadastroObserver: Observer<CadastrarTaxaResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(`A taxa "${model.nome}" foi cadastrada com sucesso!`),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/taxas']),
    };

    this.taxaService.cadastrar(model).subscribe(cadastroObserver);
  }
}
