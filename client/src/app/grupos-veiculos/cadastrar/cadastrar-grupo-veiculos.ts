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
  CadastrarGrupoVeiculosModel,
  CadastrarGrupoVeiculosResponseModel,
} from '../grupo-veiculos.models';
import { GrupoVeiculosService } from '../grupo-veiculos.service';

@Component({
  selector: 'app-cadastrar-grupo-veiculos',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './cadastrar-grupo-veiculos.html',
})
export class CadastrarGrupoVeiculos {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly grupoVeiculosService = inject(GrupoVeiculosService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected grupoVeiculosForm: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required, Validators.minLength(3)]],
  });

  get nome() {
    return this.grupoVeiculosForm.get('nome');
  }

  public cadastrar() {
    if (this.grupoVeiculosForm.invalid) return;

    const grupoVeiculosModel: CadastrarGrupoVeiculosModel = this.grupoVeiculosForm.value;

    const cadastroObserver: Observer<CadastrarGrupoVeiculosResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O grupo de veículos "${grupoVeiculosModel.nome}" foi cadastrado com sucesso!`,
        ),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/grupos-veiculos']),
    };

    this.grupoVeiculosService.cadastrar(grupoVeiculosModel).subscribe(cadastroObserver);
  }
}
