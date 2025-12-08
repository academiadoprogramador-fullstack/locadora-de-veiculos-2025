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
  DetalhesGrupoVeiculosModel,
  EditarGrupoVeiculosModel,
  EditarGrupoVeiculosResponseModel,
} from '../grupo-veiculos.models';
import { GrupoVeiculosService } from '../grupo-veiculos.service';

@Component({
  selector: 'app-editar-grupo-veiculos',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    RouterLink,
    AsyncPipe,
  ],
  templateUrl: './editar-grupo-veiculos.html',
})
export class EditarGrupoVeiculos {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly grupoVeiculosService = inject(GrupoVeiculosService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected grupoVeiculosForm: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required, Validators.minLength(3)]],
  });

  get nome() {
    return this.grupoVeiculosForm.get('nome');
  }

  protected readonly grupoVeiculos$ = this.route.data.pipe(
    filter((data) => data['grupoVeiculos']),
    map((data) => data['grupoVeiculos'] as DetalhesGrupoVeiculosModel),
    tap((grupoVeiculos) => this.grupoVeiculosForm.patchValue(grupoVeiculos)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public editar() {
    if (this.grupoVeiculosForm.invalid) return;

    const grupoVeiculosModel: EditarGrupoVeiculosModel = this.grupoVeiculosForm.value;

    const edicaoObserver: Observer<EditarGrupoVeiculosResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O grupo de veículos "${grupoVeiculosModel.nome}" foi editado com sucesso!`,
        ),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/grupos-veiculos']),
    };

    this.grupoVeiculos$
      .pipe(
        take(1),
        switchMap((grupoVeiculos) =>
          this.grupoVeiculosService.editar(grupoVeiculos.id, grupoVeiculosModel),
        ),
      )
      .subscribe(edicaoObserver);
  }
}
