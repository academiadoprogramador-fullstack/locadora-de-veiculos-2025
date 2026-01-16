import { provideNgxMask } from 'ngx-mask';

import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Routes } from '@angular/router';

import { GrupoVeiculosService } from '../grupos-veiculos/grupo-veiculos.service';
import { CadastrarVeiculo } from './cadastrar/cadastrar-veiculo';
import { EditarVeiculo } from './editar/editar-veiculo';
import { ExcluirVeiculo } from './excluir/excluir-veiculo';
import { ListarVeiculos } from './listar/listar-veiculos';
import { VeiculoService } from './veiculo.service';

export const listarVeiculosResolver = () => {
  return inject(VeiculoService).selecionarTodos();
};

export const detalhesVeiculoResolver = (route: ActivatedRouteSnapshot) => {
  const veiculoService = inject(VeiculoService);

  if (!route.paramMap.get('id')) throw new Error('O parâmetro id não foi fornecido.');

  const veiculoId = route.paramMap.get('id')!;

  return veiculoService.selecionarPorId(veiculoId);
};

export const listarGrupoVeiculosResolver = () => {
  return inject(GrupoVeiculosService).selecionarTodos();
};

export const veiculoRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarVeiculos,
        resolve: {
          veiculos: listarVeiculosResolver,
        },
      },
      {
        path: 'cadastrar',
        component: CadastrarVeiculo,
        resolve: { gruposVeiculos: listarGrupoVeiculosResolver },
      },
      {
        path: 'editar/:id',
        component: EditarVeiculo,
        resolve: { veiculo: detalhesVeiculoResolver, gruposVeiculos: listarGrupoVeiculosResolver },
      },
      {
        path: 'excluir/:id',
        component: ExcluirVeiculo,
        resolve: { veiculo: detalhesVeiculoResolver },
      },
    ],
    providers: [VeiculoService, GrupoVeiculosService, provideNgxMask()],
  },
];
