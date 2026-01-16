import { provideNgxMask } from 'ngx-mask';

import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Routes } from '@angular/router';

import { GrupoVeiculosService } from '../grupos-veiculos/grupo-veiculos.service';
import { CadastrarPlanoCobranca } from './cadastrar/cadastrar-plano-cobranca';
import { EditarPlanoCobranca } from './editar/editar-plano-cobranca';
import { ExcluirPlanoCobranca } from './excluir/excluir-plano-cobranca';
import { ListarPlanosCobranca } from './listar/listar-planos-cobranca';
import { PlanoCobrancaService } from './plano-cobranca.service';

export const listarPlanosCobrancaResolver = () => {
  return inject(PlanoCobrancaService).selecionarTodos();
};

export const detalhesPlanoCobrancaResolver = (route: ActivatedRouteSnapshot) => {
  const planoCobrancaService = inject(PlanoCobrancaService);

  if (!route.paramMap.get('id')) throw new Error('O parâmetro id não foi fornecido.');

  const planoCobrancaId = route.paramMap.get('id')!;

  return planoCobrancaService.selecionarPorId(planoCobrancaId);
};

export const listarGruposVeiculosResolver = () => {
  return inject(GrupoVeiculosService).selecionarTodos();
};

export const planoCobrancaRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarPlanosCobranca,
        resolve: {
          planosCobranca: listarPlanosCobrancaResolver,
        },
      },
      {
        path: 'cadastrar',
        component: CadastrarPlanoCobranca,
        resolve: { gruposVeiculos: listarGruposVeiculosResolver },
      },
      {
        path: 'editar/:id',
        component: EditarPlanoCobranca,
        resolve: { planoCobranca: detalhesPlanoCobrancaResolver },
      },
      {
        path: 'excluir/:id',
        component: ExcluirPlanoCobranca,
        resolve: { planoCobranca: detalhesPlanoCobrancaResolver },
      },
    ],
    providers: [PlanoCobrancaService, GrupoVeiculosService, provideNgxMask()],
  },
];
