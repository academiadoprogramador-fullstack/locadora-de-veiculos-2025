import { format, parse } from 'date-fns';
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
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  DetalhesFuncionarioModel,
  EditarFuncionarioModel,
  EditarFuncionarioResponseModel,
} from '../funcionario.models';
import { FuncionarioService } from '../funcionario.service';

@Component({
  selector: 'app-editar-funcionario',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
    NgxMaskDirective,
    AsyncPipe,
  ],
  templateUrl: './editar-funcionario.html',
})
export class EditarFuncionario {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly funcionarioService = inject(FuncionarioService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected funcionarioForm: FormGroup = this.formBuilder.group({
    nomeCompleto: ['', [Validators.required, Validators.minLength(3)]],
    cpf: ['', [Validators.required, Validators.pattern(/^\d{3}\.\d{3}\.\d{3}-\d{2}$/)]],
    salario: [0.0, [Validators.required, Validators.min(1)]],
    admissaoEmUtc: [format(new Date(), 'dd/MM/yyyy'), [Validators.required]],
  });

  get nomeCompleto() {
    return this.funcionarioForm.get('nomeCompleto');
  }

  get cpf() {
    return this.funcionarioForm.get('cpf');
  }

  get salario() {
    return this.funcionarioForm.get('salario');
  }

  get admissaoEmUtc() {
    return this.funcionarioForm.get('admissaoEmUtc');
  }

  protected readonly funcionario$ = this.route.data.pipe(
    filter((data) => data['funcionario']),
    map((data) => data['funcionario'] as DetalhesFuncionarioModel),
    tap((funcionario) => this.funcionarioForm.patchValue(funcionario)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.funcionarioForm.invalid) return;

    const funcionarioModel: EditarFuncionarioModel = {
      ...this.funcionarioForm.value,
      admissaoEmUtc: parse(this.admissaoEmUtc?.value, 'dd/MM/yyyy', new Date()),
    };

    const edicaoObserver: Observer<EditarFuncionarioResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O funcionário "${funcionarioModel.nomeCompleto}" foi editado com sucesso!`,
        ),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/funcionarios']),
    };

    this.funcionario$
      .pipe(
        take(1),
        switchMap((funcionario) =>
          this.funcionarioService.editar(funcionario.id, funcionarioModel),
        ),
      )
      .subscribe(edicaoObserver);
  }
}
