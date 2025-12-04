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
