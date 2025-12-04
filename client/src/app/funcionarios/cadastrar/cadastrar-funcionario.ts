import { format, parse } from 'date-fns';
import { NgxMaskDirective } from 'ngx-mask';
import { Observer } from 'rxjs';

import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';

import { compararConfirmacaoSenha } from '../../auth/validators/comparar-confirmacao-senha-validator';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  CadastrarFuncionarioModel,
  CadastrarFuncionarioResponseModel,
} from '../funcionario.models';
import { FuncionarioService } from '../funcionario.service';

@Component({
  selector: 'app-cadastrar-funcionario',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
    NgxMaskDirective,
  ],
  templateUrl: './cadastrar-funcionario.html',
})
export class CadastrarFuncionario {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly funcionarioService = inject(FuncionarioService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected funcionarioForm: FormGroup = this.formBuilder.group(
    {
      nomeCompleto: ['', [Validators.required, Validators.minLength(3)]],
      cpf: ['', [Validators.required, Validators.pattern(/^\d{3}\.\d{3}\.\d{3}-\d{2}$/)]],
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6)]],
      confirmarSenha: ['', [Validators.required, Validators.minLength(6)]],
      salario: [0.0, [Validators.required, Validators.min(1)]],
      admissaoEmUtc: [format(new Date(), 'dd/MM/yyyy'), [Validators.required]],
    },
    { validators: [compararConfirmacaoSenha] },
  );

  get nomeCompleto() {
    return this.funcionarioForm.get('nomeCompleto');
  }

  get cpf() {
    return this.funcionarioForm.get('cpf');
  }

  get email() {
    return this.funcionarioForm.get('email');
  }

  get senha() {
    return this.funcionarioForm.get('senha');
  }

  get confirmarSenha() {
    return this.funcionarioForm.get('confirmarSenha');
  }

  get salario() {
    return this.funcionarioForm.get('salario');
  }

  get admissaoEmUtc() {
    return this.funcionarioForm.get('admissaoEmUtc');
  }

  public cadastrar() {
    if (this.funcionarioForm.invalid) return;

    const funcionarioModel: CadastrarFuncionarioModel = {
      ...this.funcionarioForm.value,
      admissaoEmUtc: parse(this.admissaoEmUtc?.value, 'dd/MM/yyyy', new Date()),
    };

    const cadastroObserver: Observer<CadastrarFuncionarioResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O funcionário "${funcionarioModel.nomeCompleto}" foi cadastrado com sucesso!`,
        ),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/funcionarios']),
    };

    this.funcionarioService.cadastrar(funcionarioModel).subscribe(cadastroObserver);
  }
}
