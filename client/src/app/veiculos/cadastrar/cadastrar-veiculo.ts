import { NgxMaskDirective } from 'ngx-mask';
import { filter, map, Observer, shareReplay } from 'rxjs';

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
    CadastrarVeiculoModel, CadastrarVeiculoResponseModel, TipoCombustivel
} from '../veiculo.models';
import { VeiculoService } from '../veiculo.service';

@Component({
  selector: 'app-cadastrar-veiculo',
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
  templateUrl: './cadastrar-veiculo.html',
})
export class CadastrarVeiculo {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly veiculoService = inject(VeiculoService);
  protected readonly notificacaoService = inject(NotificacaoService);

  protected readonly tipoCombustivelEnum = Object.values(TipoCombustivel);

  protected imagemPreview = signal<string | null>(null); // signal = versão "nativa" do BehaviorSubject

  protected readonly gruposVeiculos$ = this.route.data.pipe(
    filter((data) => data['gruposVeiculos']),
    map((data) => data['gruposVeiculos'] as SelecionarGruposVeiculosModel[]),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected veiculoForm: FormGroup = this.formBuilder.group({
    grupoVeiculosId: [null, [Validators.required]],
    modelo: ['', [Validators.required, Validators.minLength(3)]],
    marca: ['', [Validators.required]],
    ano: [0, [Validators.required]],
    capacidadeTanque: [0, [Validators.required]],
    tipoCombustivel: [TipoCombustivel.Alcool, [Validators.required]],
    imagem: [null, [Validators.required]],
  });

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
      this.imagem?.updateValueAndValidity();

      const reader = new FileReader();

      reader.onload = () => {
        this.imagemPreview.set(reader.result as string);
      };

      reader.readAsDataURL(file);
    }
  }

  public removerImagem() {
    this.imagemPreview.set(null);
    this.veiculoForm.patchValue({ imagem: null });
  }

  public cadastrar() {
    if (this.veiculoForm.invalid) return;

    const veiculoModel: CadastrarVeiculoModel = this.veiculoForm.value;

    const cadastroObserver: Observer<CadastrarVeiculoResponseModel> = {
      next: () =>
        this.notificacaoService.sucesso(
          `O veículo "${veiculoModel.marca} ${veiculoModel.modelo}" foi cadastrado com sucesso!`,
        ),
      error: (err) => this.notificacaoService.erro(err),
      complete: () => this.router.navigate(['/veiculos']),
    };

    this.veiculoService.cadastrar(veiculoModel).subscribe(cadastroObserver);
  }
}
