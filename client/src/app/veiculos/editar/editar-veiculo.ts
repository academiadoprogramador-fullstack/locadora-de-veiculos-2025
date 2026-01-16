import { NgxMaskDirective } from 'ngx-mask';
import { filter, map, Observer, shareReplay, switchMap, take, tap } from 'rxjs';

import { AsyncPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { SelecionarGruposVeiculosModel } from '../../grupos-veiculos/grupo-veiculos.models';
import { NotificacaoService } from '../../shared/notificacao/notificacao.service';
import {
  DetalhesVeiculoModel,
  EditarVeiculoResponseModel,
  TipoCombustivel,
} from '../veiculo.models';
import { VeiculoService } from '../veiculo.service';

@Component({
  selector: 'app-editar-veiculo',
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
  templateUrl: './editar-veiculo.html',
})
export class EditarVeiculo {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly veiculoService = inject(VeiculoService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly tipoCombustivelEnum = Object.values(TipoCombustivel);

  protected imagemPreview = signal<string | null>(null);

  protected readonly gruposVeiculos$ = this.route.data.pipe(
    filter((data) => data['gruposVeiculos']),
    map((data) => data['gruposVeiculos'] as SelecionarGruposVeiculosModel[]),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected veiculoForm: FormGroup = this.formBuilder.group({
    id: [0, [Validators.required]], // ID é necessário para edição
    grupoVeiculosId: [null, [Validators.required]],
    modelo: ['', [Validators.required, Validators.minLength(3)]],
    marca: ['', [Validators.required]],
    ano: [null, [Validators.required]],
    capacidadeTanque: [null, [Validators.required]],
    tipoCombustivel: [null, [Validators.required]],
    imagem: [null],
  });

  protected readonly urlPublicoS3 = 'https://pub-ed252ae2497a49449f8c2159a5331e5f.r2.dev/';

  protected readonly veiculo$ = this.route.data.pipe(
    filter((data) => data['veiculo']),
    map((data) => data['veiculo'] as DetalhesVeiculoModel),
    tap((veiculo) => {
      this.veiculoForm.patchValue({ ...veiculo, grupoVeiculosId: veiculo.grupoVeiculos.id });

      if (veiculo.imagem) {
        this.imagemPreview.set(this.urlPublicoS3 + veiculo.imagem);
      }
    }),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  get grupoVeiculosId() {
    return this.veiculoForm.get('grupoVeiculosId');
  }

  get modelo() {
    return this.veiculoForm.get('modelo');
  }

  get marca() {
    return this.veiculoForm.get('marca');
  }

  get ano() {
    return this.veiculoForm.get('ano');
  }

  get capacidadeTanque() {
    return this.veiculoForm.get('capacidadeTanque');
  }

  get tipoCombustivel() {
    return this.veiculoForm.get('tipoCombustivel');
  }

  get imagem() {
    return this.veiculoForm.get('imagem');
  }

  public onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      const file = input.files[0];

      this.veiculoForm.patchValue({ imagem: file });
      this.veiculoForm.get('imagem')?.markAsTouched();

      const reader = new FileReader();

      reader.onload = () => {
        this.imagemPreview.set(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  }

  public editar() {
    if (this.veiculoForm.invalid) return;

    const veiculoModel = this.veiculoForm.value;

    const edicaoObserver: Observer<EditarVeiculoResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O veículo "${veiculoModel.marca} ${veiculoModel.modelo}" foi editado com sucesso!`,
        ),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/veiculos']),
    };

    this.veiculo$
      .pipe(
        take(1),
        switchMap((veiculoOriginal) =>
          this.veiculoService.editar(veiculoOriginal.id, veiculoModel),
        ),
      )
      .subscribe(edicaoObserver);
  }
}
