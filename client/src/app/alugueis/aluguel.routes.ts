import { provideNgxMask } from 'ngx-mask';

import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Routes } from '@angular/router';

import { CondutorService } from '../condutores/condutor.service';
import {
    ConfiguracaoCombustiveisService
} from '../configuracoes-combustiveis/configuracao-combustiveis.service';
import { TaxaService } from '../taxas/taxa.service';
import { VeiculoService } from '../veiculos/veiculo.service';
import { AbrirAluguel } from './abrir/abrir-aluguel';
import { AluguelService } from './aluguel.service';
import { ConcluirAluguel } from './concluir/concluir-aluguel';
import { DetalhesAluguel } from './detalhes/detalhes-aluguel';
import { EditarAluguel } from './editar/editar-aluguel';
import { ExcluirAluguel } from './excluir/excluir-aluguel';
import { ListarAlugueis } from './listar/listar-alugueis';
import { SimularAberturaAluguel } from './simular-abertura/simular-abertura-aluguel';

export const listarAlugueisResolver = () => {
  return inject(AluguelService).selecionarTodos();
};

export const detalhesAluguelResolver = (route: ActivatedRouteSnapshot) => {
  const aluguelService = inject(AluguelService);

  const id = route.paramMap.get('id');
  if (!id) throw new Error('O parâmetro id não foi fornecido.');

  return aluguelService.selecionarPorId(id);
};

export const listarCondutoresResolver = () => {
  return inject(CondutorService).selecionarTodos();
};

export const listarVeiculosResolver = () => {
  return inject(VeiculoService).selecionarTodos();
};

export const obterUltimaConfiguracaoCombustiveisResolver = () => {
  return inject(ConfiguracaoCombustiveisService).selecionarUltima();
};

export const listarTaxasResolver = () => {
  return inject(TaxaService).selecionarTodos();
};

export const aluguelRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarAlugueis,
        resolve: {
          alugueis: listarAlugueisResolver,
        },
      },
      {
        path: 'simular-abertura',
        component: SimularAberturaAluguel,
        resolve: {
          condutores: listarCondutoresResolver,
          veiculos: listarVeiculosResolver,
          taxas: listarTaxasResolver,
          configuracaoCombustiveis: obterUltimaConfiguracaoCombustiveisResolver,
        },
      },
      {
        path: 'abrir/:id',
        component: AbrirAluguel,
      },
      {
        path: 'concluir/:id',
        component: ConcluirAluguel,
        resolve: {
          aluguel: detalhesAluguelResolver,
          taxas: listarTaxasResolver,
        },
      },
      {
        path: 'editar/:id',
        component: EditarAluguel,
        resolve: {
          aluguel: detalhesAluguelResolver,
          condutores: listarCondutoresResolver,
          veiculos: listarVeiculosResolver,
          taxas: listarTaxasResolver,
        },
      },
      {
        path: 'excluir/:id',
        component: ExcluirAluguel,
        resolve: { aluguel: detalhesAluguelResolver },
      },
      {
        path: 'detalhes/:id',
        component: DetalhesAluguel,
        resolve: { aluguel: detalhesAluguelResolver },
      },
    ],
    providers: [
      AluguelService,
      CondutorService,
      VeiculoService,
      TaxaService,
      ConfiguracaoCombustiveisService,
      provideNgxMask(),
    ],
  },
];
