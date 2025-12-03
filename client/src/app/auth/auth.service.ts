import {
    BehaviorSubject, catchError, defer, distinctUntilChanged, Observable, of, shareReplay,
    switchMap, tap
} from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import { AccessTokenModel, LoginModel, RegistroModel } from './auth.models';

@Injectable()
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/auth';

  private readonly accessTokenSubject$ = new BehaviorSubject<AccessTokenModel | null>(null);

  private readonly inicializacao$ = defer(() =>
    this.rotacionar().pipe(catchError(() => of(null))),
  ).pipe(shareReplay({ bufferSize: 1, refCount: true }));

  public obterAccessToken(): Observable<AccessTokenModel | null> {
    return this.inicializacao$.pipe(
      switchMap(() => this.accessTokenSubject$.asObservable()),
      distinctUntilChanged((a, b) => a?.chave === b?.chave),
    );
  }

  public registro(registroModel: RegistroModel): Observable<AccessTokenModel> {
    const urlCompleto = `${this.apiUrl}/registrar`;

    return this.http
      .post<AccessTokenModel>(urlCompleto, registroModel)
      .pipe(tap((token) => this.accessTokenSubject$.next(token)));
  }

  public login(loginModel: LoginModel): Observable<AccessTokenModel> {
    const urlCompleto = `${this.apiUrl}/autenticar`;

    return this.http
      .post<AccessTokenModel>(urlCompleto, loginModel)
      .pipe(tap((token) => this.accessTokenSubject$.next(token)));
  }

  public rotacionar(): Observable<AccessTokenModel> {
    const urlCompleto = `${this.apiUrl}/rotacionar`;

    return this.http
      .post<AccessTokenModel>(urlCompleto, {})
      .pipe(tap((token) => this.accessTokenSubject$.next(token)));
  }

  public sair(): Observable<null> {
    const urlCompleto = `${this.apiUrl}/sair`;

    return this.http.post<null>(urlCompleto, {}).pipe(tap(() => this.revogarAccessToken()));
  }

  public revogarAccessToken(): void {
    return this.accessTokenSubject$.next(null);
  }
}
