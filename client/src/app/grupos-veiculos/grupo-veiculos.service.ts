import { map, Observable } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import {
  CadastrarGrupoVeiculosModel,
  CadastrarGrupoVeiculosResponseModel,
  DetalhesGrupoVeiculosModel,
  EditarGrupoVeiculosModel,
  EditarGrupoVeiculosResponseModel,
  SelecionarGruposVeiculosModel,
  SelecionarGruposVeiculosResponseModel,
} from './grupo-veiculos.models';

@Injectable()
export class GrupoVeiculosService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/grupos-veiculos';

  public cadastrar(
    grupoVeiculosModel: CadastrarGrupoVeiculosModel,
  ): Observable<CadastrarGrupoVeiculosResponseModel> {
    return this.http.post<CadastrarGrupoVeiculosResponseModel>(this.apiUrl, grupoVeiculosModel);
  }

  public editar(
    id: string,
    grupoVeiculosModel: EditarGrupoVeiculosModel,
  ): Observable<EditarGrupoVeiculosResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.put<EditarGrupoVeiculosResponseModel>(urlCompleto, grupoVeiculosModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarPorId(id: string): Observable<DetalhesGrupoVeiculosModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.get<DetalhesGrupoVeiculosModel>(urlCompleto);
  }

  public selecionarTodos(): Observable<SelecionarGruposVeiculosModel[]> {
    return this.http
      .get<SelecionarGruposVeiculosResponseModel>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }
}
