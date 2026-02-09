import { map, Observable } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import {
  CadastrarConfiguracaoCombustiveisModel,
  CadastrarConfiguracaoCombustiveisResponseModel,
  SelecionarConfiguracoesCombustiveisModel,
  SelecionarConfiguracoesCombustiveisResponseModel,
  SelecionarUltimaConfiguracaoCombustiveisResponseModel,
} from './configuracao-combustiveis.models';

@Injectable()
export class ConfiguracaoCombustiveisService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/configuracoes-combustiveis';

  public cadastrar(
    model: CadastrarConfiguracaoCombustiveisModel,
  ): Observable<CadastrarConfiguracaoCombustiveisResponseModel> {
    return this.http.post<CadastrarConfiguracaoCombustiveisResponseModel>(this.apiUrl, model);
  }

  public selecionarTodas(): Observable<SelecionarConfiguracoesCombustiveisModel[]> {
    return this.http
      .get<SelecionarConfiguracoesCombustiveisResponseModel>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }

  public selecionarUltima(): Observable<SelecionarUltimaConfiguracaoCombustiveisResponseModel> {
    const urlCompleto = `${this.apiUrl}/ultima`;

    return this.http.get<SelecionarUltimaConfiguracaoCombustiveisResponseModel>(urlCompleto);
  }
}
