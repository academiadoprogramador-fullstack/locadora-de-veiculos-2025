import { catchError, switchMap, take, throwError } from 'rxjs';

import { HttpErrorResponse, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

import { NotificacaoService } from '../shared/notificacao/notificacao.service';
import { AuthService } from './auth.service';

export const authInterceptor = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const notificacaoService = inject(NotificacaoService);

  const whitelist = ['/auth/autenticar', '/auth/registrar', '/auth/rotacionar', '/auth/sair'];

  if (whitelist.some((url) => req.url.includes(url)))
    return next(req.clone({ withCredentials: true }));

  return authService.obterAccessToken().pipe(
    take(1),
    switchMap((accessToken) => {
      if (!accessToken) return next(req.clone({ withCredentials: true }));

      const requisicaoComAccessToken = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${accessToken.chave}`),
        withCredentials: true,
      });

      return next(requisicaoComAccessToken).pipe(
        catchError((err) => {
          if ((err as HttpErrorResponse).status !== 401) return throwError(() => err);

          // Tentativa de renovar o access token usando o refresh token (cookie)
          return authService.rotacionar().pipe(
            switchMap((novoAccessToken) => {
              const requisicaoRenovada = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${novoAccessToken.chave}`),
                withCredentials: true,
              });

              return next(requisicaoRenovada);
            }),
            catchError((refreshError) => {
              notificacaoService.erro('Sua sessão expirou. Por favor, faça login novamente.');

              authService.revogarAccessToken();

              router.navigate(['/auth', 'login']);

              return throwError(() => refreshError);
            }),
          );
        }),
      );
    }),
  );
};
