import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Routes } from '@angular/router';

import { CadastrarGrupoVeiculos } from './cadastrar/cadastrar-grupo-veiculos';
import { EditarGrupoVeiculos } from './editar/editar-grupo-veiculos';
import { ExcluirGrupoVeiculos } from './excluir/excluir-grupo-veiculos';
import { GrupoVeiculosService } from './grupo-veiculos.service';
import { ListarGruposVeiculos } from './listar/listar-grupos-veiculos';

export const listarGrupoVeiculosResolver = () => {
  return inject(GrupoVeiculosService).selecionarTodos();
};

export const detalhesGrupoVeiculosResolver = (route: ActivatedRouteSnapshot) => {
  const grupoVeiculosService = inject(GrupoVeiculosService);

  if (!route.paramMap.get('id')) throw new Error('O parâmetro id não foi fornecido.');

  const grupoVeiculosId = route.paramMap.get('id')!;

  return grupoVeiculosService.selecionarPorId(grupoVeiculosId);
};

export const grupoVeiculosRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarGruposVeiculos,
        resolve: { gruposVeiculos: listarGrupoVeiculosResolver },
      },
      {
        path: 'cadastrar',
        component: CadastrarGrupoVeiculos,
      },
      {
        path: 'editar/:id',
        component: EditarGrupoVeiculos,
        resolve: { grupoVeiculos: detalhesGrupoVeiculosResolver },
      },
      {
        path: 'excluir/:id',
        component: ExcluirGrupoVeiculos,
        resolve: { grupoVeiculos: detalhesGrupoVeiculosResolver },
      },
    ],
    providers: [GrupoVeiculosService],
  },
];
