import { provideNgxMask } from 'ngx-mask';

import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Routes } from '@angular/router';

import { CadastrarFuncionario } from './cadastrar/cadastrar-funcionario';
import { EditarFuncionario } from './editar/editar-funcionario';
import { ExcluirFuncionario } from './excluir/excluir-funcionario';
import { FuncionarioService } from './funcionario.service';
import { ListarFuncionarios } from './listar/listar-funcionarios';

export const listarFuncionariosResolver = () => {
  return inject(FuncionarioService).selecionarTodos();
};

export const detalhesFuncionarioResolver = (route: ActivatedRouteSnapshot) => {
  const medicoService = inject(FuncionarioService);

  if (!route.paramMap.get('id')) throw new Error('O parâmetro id não foi fornecido.');

  const medicoId = route.paramMap.get('id')!;

  return medicoService.selecionarPorId(medicoId);
};

export const funcionarioRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarFuncionarios,
        resolve: { funcionarios: listarFuncionariosResolver },
      },
      {
        path: 'cadastrar',
        component: CadastrarFuncionario,
      },
      {
        path: 'editar/:id',
        component: EditarFuncionario,
        resolve: { funcionario: detalhesFuncionarioResolver },
      },
      {
        path: 'excluir/:id',
        component: ExcluirFuncionario,
        resolve: { funcionario: detalhesFuncionarioResolver },
      },
    ],
    providers: [FuncionarioService, provideNgxMask()],
  },
];
