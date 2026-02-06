import { NgxMaskDirective } from 'ngx-mask';
import { Observer } from 'rxjs';

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
    CadastrarClienteModel, CadastrarClienteResponseModel, TipoCliente
} from '../cliente.models';
import { ClienteService } from '../cliente.service';

@Component({
  selector: 'app-cadastrar-cliente',
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
  ],
  templateUrl: './cadastrar-cliente.html',
})
export class CadastrarCliente {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly clienteService = inject(ClienteService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly tipoClienteEnum = Object.values(TipoCliente);

  protected clienteForm: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    telefone: ['', [Validators.required]],
    tipo: [TipoCliente.Cpf, [Validators.required]],
    numeroDocumento: ['', [Validators.required]],
    cidade: ['', [Validators.required]],
    estado: ['', [Validators.required]],
    bairro: ['', [Validators.required]],
    rua: ['', [Validators.required]],
    numero: ['', [Validators.required]],
  });

  get nome() {
    return this.clienteForm.get('nome');
  }

  get email() {
    return this.clienteForm.get('email');
  }

  get telefone() {
    return this.clienteForm.get('telefone');
  }

  get tipo() {
    return this.clienteForm.get('tipo');
  }

  get numeroDocumento() {
    return this.clienteForm.get('numeroDocumento');
  }

  get cidade() {
    return this.clienteForm.get('cidade');
  }

  get estado() {
    return this.clienteForm.get('estado');
  }

  get bairro() {
    return this.clienteForm.get('bairro');
  }

  get rua() {
    return this.clienteForm.get('rua');
  }

  get numero() {
    return this.clienteForm.get('numero');
  }

  public cadastrar() {
    if (this.clienteForm.invalid) return;

    const clienteModel: CadastrarClienteModel = this.clienteForm.value;

    const cadastroObserver: Observer<CadastrarClienteResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O cliente "${clienteModel.nome}" foi cadastrado com sucesso!`,
        ),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/clientes']),
    };

    this.clienteService.cadastrar(clienteModel).subscribe(cadastroObserver);
  }
}
