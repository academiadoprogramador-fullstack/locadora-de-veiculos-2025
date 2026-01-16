import { map, Observable } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import {
  CadastrarPlanoCobrancaModel,
  CadastrarPlanoCobrancaResponseModel,
  DetalhesPlanoCobrancaModel,
  EditarPlanoCobrancaModel,
  EditarPlanoCobrancaResponseModel,
  SelecionarPlanosCobrancaModel,
  SelecionarPlanosCobrancaResponseModel,
} from './plano-cobranca.models';

@Injectable()
export class PlanoCobrancaService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/planos-cobranca';

  public cadastrar(
    grupoVeiculosModel: CadastrarPlanoCobrancaModel,
  ): Observable<CadastrarPlanoCobrancaResponseModel> {
    return this.http.post<CadastrarPlanoCobrancaResponseModel>(this.apiUrl, grupoVeiculosModel);
  }

  public editar(
    id: string,
    grupoVeiculosModel: EditarPlanoCobrancaModel,
  ): Observable<EditarPlanoCobrancaResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.put<EditarPlanoCobrancaResponseModel>(urlCompleto, grupoVeiculosModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarPorId(id: string): Observable<DetalhesPlanoCobrancaModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.get<DetalhesPlanoCobrancaModel>(urlCompleto);
  }

  public selecionarTodos(): Observable<SelecionarPlanosCobrancaModel[]> {
    return this.http
      .get<SelecionarPlanosCobrancaResponseModel>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }
}
