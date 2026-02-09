import { inject } from '@angular/core';
import { Routes } from '@angular/router';

import { CadastrarConfiguracaoCombustiveis } from './cadastrar/cadastrar-configuracao-combustiveis';
import { ConfiguracaoCombustiveisService } from './configuracao-combustiveis.service';
import { ListarConfiguracoesCombustiveis } from './listar/listar-configuracoes-combustiveis';

export const listarConfiguracoesCombustiveisResolver = () => {
  return inject(ConfiguracaoCombustiveisService).selecionarTodas();
};

export const configuracaoCombustiveisRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarConfiguracoesCombustiveis,
        resolve: { configuracoes: listarConfiguracoesCombustiveisResolver },
      },
      {
        path: 'cadastrar',
        component: CadastrarConfiguracaoCombustiveis,
      },
    ],
    providers: [ConfiguracaoCombustiveisService],
  },
];
