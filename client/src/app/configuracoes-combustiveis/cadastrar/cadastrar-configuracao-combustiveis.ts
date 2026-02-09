import { Observer } from 'rxjs';

import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';

import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  CadastrarConfiguracaoCombustiveisModel,
  CadastrarConfiguracaoCombustiveisResponseModel,
} from '../configuracao-combustiveis.models';
import { ConfiguracaoCombustiveisService } from '../configuracao-combustiveis.service';

@Component({
  selector: 'app-cadastrar-configuracao-combustiveis',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './cadastrar-configuracao-combustiveis.html',
})
export class CadastrarConfiguracaoCombustiveis {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly configuracaoService = inject(ConfiguracaoCombustiveisService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected configuracaoForm: FormGroup = this.formBuilder.group({
    valorGasolina: [0, [Validators.required, Validators.min(0.01)]],
    valorAlcool: [0, [Validators.required, Validators.min(0.01)]],
    valorDiesel: [0, [Validators.required, Validators.min(0.01)]],
    valorGas: [0, [Validators.required, Validators.min(0.01)]],
    valorEletricidade: [0, [Validators.required, Validators.min(0.01)]],
  });

  get valorGasolina() {
    return this.configuracaoForm.get('valorGasolina');
  }

  get valorAlcool() {
    return this.configuracaoForm.get('valorAlcool');
  }

  get valorDiesel() {
    return this.configuracaoForm.get('valorDiesel');
  }

  get valorGas() {
    return this.configuracaoForm.get('valorGas');
  }

  get valorEletricidade() {
    return this.configuracaoForm.get('valorEletricidade');
  }

  public cadastrar() {
    if (this.configuracaoForm.invalid) return;

    const model: CadastrarConfiguracaoCombustiveisModel = this.configuracaoForm.value;

    const cadastroObserver: Observer<CadastrarConfiguracaoCombustiveisResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `A configuração de combustíveis foi cadastrada com sucesso!`,
        ),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/configuracoes-combustiveis']),
    };

    this.configuracaoService.cadastrar(model).subscribe(cadastroObserver);
  }
}
