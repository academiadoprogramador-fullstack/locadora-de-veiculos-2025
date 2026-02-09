import { format, parse } from 'date-fns';
import { NgxMaskDirective } from 'ngx-mask';
import { combineLatest, filter, map, Observer, shareReplay, startWith } from 'rxjs';

import { AsyncPipe } from '@angular/common';
import { Component, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { SelecionarClientesModel, TipoCliente } from '../../clientes/cliente.models';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import { CadastrarCondutorModel, CadastrarCondutorResponseModel } from '../condutor.models';
import { CondutorService } from '../condutor.service';

@Component({
  selector: 'app-cadastrar-condutor',
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
  templateUrl: './cadastrar-condutor.html',
})
export class CadastrarCondutor {
  private readonly destroyRef = inject(DestroyRef);

  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly condutorService = inject(CondutorService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly clientes$ = this.route.data.pipe(
    filter((data) => data['clientes']),
    map((data) => data['clientes'] as SelecionarClientesModel[]),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected condutorForm: FormGroup = this.formBuilder.group({
    clienteId: [null, [Validators.required]],
    clienteCondutor: [false, [Validators.required]],
    nome: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    telefone: ['', [Validators.required]],
    cpf: ['', [Validators.required]],
    cnh: ['', [Validators.required]],
    validadeCnh: [format(new Date(), 'dd/MM/yyyy'), [Validators.required]],
  });

  get clienteId() {
    return this.condutorForm.get('clienteId');
  }

  get clienteCondutor() {
    return this.condutorForm.get('clienteCondutor');
  }

  get nome() {
    return this.condutorForm.get('nome');
  }

  get email() {
    return this.condutorForm.get('email');
  }

  get telefone() {
    return this.condutorForm.get('telefone');
  }

  get cpf() {
    return this.condutorForm.get('cpf');
  }

  get cnh() {
    return this.condutorForm.get('cnh');
  }

  get validadeCnh() {
    return this.condutorForm.get('validadeCnh');
  }

  constructor() {
    const clienteId$ = this.clienteId!.valueChanges.pipe(startWith(this.clienteId!.value));

    const clienteCondutor$ = this.clienteCondutor!.valueChanges.pipe(
      startWith(this.clienteCondutor!.value),
    );

    // Toda vez que clienteId/clienteCondutor mudar (ou ao iniciar), aplicamos as regras.
    combineLatest([this.clientes$, clienteId$, clienteCondutor$])
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(([clientes]) => this.aplicarRegrasClienteCondutor(clientes));
  }

  public cadastrar() {
    if (this.condutorForm.invalid) return;

    const rawValue = this.condutorForm.getRawValue();

    const model: CadastrarCondutorModel = {
      ...rawValue,
      validadeCnh: parse(rawValue.validadeCnh, 'dd/MM/yyyy', new Date()),
    };

    const cadastroObserver: Observer<CadastrarCondutorResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(`O condutor "${model.nome}" foi cadastrado com sucesso!`),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/condutores']),
    };

    this.condutorService.cadastrar(model).subscribe(cadastroObserver);
  }

  private aplicarRegrasClienteCondutor(clientes: SelecionarClientesModel[]) {
    const clienteId = this.clienteId?.value;
    if (!clienteId) return;

    const cliente = clientes.find((c) => c.id === clienteId);
    if (!cliente) return;

    const clienteCnpj = cliente.tipo === TipoCliente.Cnpj;

    // Campos que podem ser preenchidos automáticamente (caso cliente seja CPF)
    const camposAuto = [this.nome!, this.email!, this.telefone!, this.cpf!];

    // 1. Se cliente é CNPJ...
    if (clienteCnpj) {
      // Se o cliente selecionado for CNPJ, resetamos o valor do campo "clienteCondutor"
      if (this.clienteCondutor!.value === true) {
        this.condutorForm.patchValue({ clienteCondutor: false }, { emitEvent: false });

        this.notificacaoService.erro(
          'Cliente do tipo CNPJ não pode ser marcado como "Cliente é condutor".',
        );
      }

      // Desabilita o campo "clienteCondutor"
      if (this.clienteCondutor!.enabled) this.clienteCondutor!.disable({ emitEvent: false });

      // Habilita os campos de preenchimento automático (como não haverá auto-preenchimento)
      camposAuto.forEach((c) => c.enable({ emitEvent: false }));

      return;
    }

    // 2. Se cliente é CPF, habilita campo "clienteCondutor"
    if (this.clienteCondutor!.disabled) this.clienteCondutor!.enable({ emitEvent: false });

    // 3. Se o valor do campo "clienteCondutor" for true...
    if (this.clienteCondutor!.value === true) {
      // Preenche os campos abaixo com os dados do cliente selecionado
      this.condutorForm.patchValue(
        {
          nome: cliente.nome,
          email: cliente.email,
          telefone: cliente.telefone,
          cpf: cliente.numeroDocumento, // CPF com máscara já vem do cliente
        },
        { emitEvent: false },
      );

      // Desabilita os campos de preenchimento automático
      camposAuto.forEach((c) => c.disable({ emitEvent: false }));

      return;
    }

    // 4. Se o valor do campo "clienteCondutor" for false...
    // Habilita os campos de preenchimento automático
    camposAuto.forEach((c) => c.enable({ emitEvent: false }));
  }
}
