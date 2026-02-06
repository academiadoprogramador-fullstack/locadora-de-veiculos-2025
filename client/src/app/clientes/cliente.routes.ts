import { provideNgxMask } from 'ngx-mask';

import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Routes } from '@angular/router';

import { CadastrarCliente } from './cadastrar/cadastrar-cliente';
import { ClienteService } from './cliente.service';
import { EditarCliente } from './editar/editar-cliente';
import { ExcluirCliente } from './excluir/excluir-cliente';
import { ListarClientes } from './listar/listar-clientes';

export const listarClientesResolver = () => {
  return inject(ClienteService).selecionarTodos();
};

export const detalhesClienteResolver = (route: ActivatedRouteSnapshot) => {
  const clienteService = inject(ClienteService);

  if (!route.paramMap.get('id')) throw new Error('O parâmetro id não foi fornecido.');

  const clienteId = route.paramMap.get('id')!;

  return clienteService.selecionarPorId(clienteId);
};

export const clienteRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ListarClientes,
        resolve: {
          clientes: listarClientesResolver,
        },
      },
      {
        path: 'cadastrar',
        component: CadastrarCliente,
      },
      {
        path: 'editar/:id',
        component: EditarCliente,
        resolve: { cliente: detalhesClienteResolver },
      },
      {
        path: 'excluir/:id',
        component: ExcluirCliente,
        resolve: { cliente: detalhesClienteResolver },
      },
    ],
    providers: [ClienteService, provideNgxMask()],
  },
];
