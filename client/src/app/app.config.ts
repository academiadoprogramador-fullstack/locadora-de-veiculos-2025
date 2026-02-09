import { map, take } from 'rxjs';

import {
  ApplicationConfig,
  DEFAULT_CURRENCY_CODE,
  inject,
  LOCALE_ID,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { CanActivateFn, provideRouter, Router, Routes } from '@angular/router';

import { provideAuth } from './auth/auth.provider';
import { AuthService } from './auth/auth.service';
import { provideNotifications } from './shared/notificacao/notificacao.provider';

const usuarioDesconhecidoGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.obterAccessToken().pipe(
    take(1),
    map((token) => (!token ? true : router.createUrlTree(['/inicio']))),
  );
};

const usuarioAutenticadoGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.obterAccessToken().pipe(
    take(1),
    map((token) => (token ? true : router.createUrlTree(['/auth/login']))),
  );
};

const routes: Routes = [
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.routes').then((r) => r.authRoutes),
    canMatch: [usuarioDesconhecidoGuard],
  },
  {
    path: 'funcionarios',
    loadChildren: () =>
      import('./funcionarios/funcionario.routes').then((r) => r.funcionarioRoutes),
    canMatch: [usuarioAutenticadoGuard],
  },
  {
    path: 'grupos-veiculos',
    loadChildren: () =>
      import('./grupos-veiculos/grupo-veiculos.routes').then((r) => r.grupoVeiculosRoutes),
    canMatch: [usuarioAutenticadoGuard],
  },
  {
    path: 'planos-cobranca',
    loadChildren: () =>
      import('./planos-cobranca/plano-cobranca.routes').then((r) => r.planoCobrancaRoutes),
    canMatch: [usuarioAutenticadoGuard],
  },
  {
    path: 'veiculos',
    loadChildren: () => import('./veiculos/veiculo.routes').then((r) => r.veiculoRoutes),
    canMatch: [usuarioAutenticadoGuard],
  },
  {
    path: 'clientes',
    loadChildren: () => import('./clientes/cliente.routes').then((r) => r.clienteRoutes),
    canMatch: [usuarioAutenticadoGuard],
  },
  {
    path: 'condutores',
    loadChildren: () => import('./condutores/condutor.routes').then((r) => r.condutorRoutes),
    canMatch: [usuarioAutenticadoGuard],
  },
  {
    path: 'taxas',
    loadChildren: () => import('./taxas/taxa.routes').then((r) => r.taxaRoutes),
    canMatch: [usuarioAutenticadoGuard],
  },
  {
    path: 'configuracoes-combustiveis',
    loadChildren: () =>
      import('./configuracoes-combustiveis/configuracao-combustiveis.routes').then(
        (r) => r.configuracaoCombustiveisRoutes,
      ),
    canMatch: [usuarioAutenticadoGuard],
  },
  {
    path: 'inicio',
    loadComponent: () => import('./inicio/inicio').then((c) => c.Inicio),
    canMatch: [usuarioAutenticadoGuard],
  },
];

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),

    { provide: LOCALE_ID, useValue: 'pt-BR' },
    { provide: DEFAULT_CURRENCY_CODE, useValue: 'BRL' },

    provideNotifications(),
    provideAuth(),
  ],
};
