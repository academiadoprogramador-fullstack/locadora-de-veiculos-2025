import { map, Observable } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import {
    CadastrarClienteModel, CadastrarClienteResponseModel, DetalhesClienteModel, EditarClienteModel,
    EditarClienteResponseModel, SelecionarClientesModel, SelecionarClientesResponseModel
} from './cliente.models';

@Injectable()
export class ClienteService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/clientes';

  public cadastrar(veiculoModel: CadastrarClienteModel): Observable<CadastrarClienteResponseModel> {
    return this.http.post<CadastrarClienteResponseModel>(this.apiUrl, veiculoModel);
  }

  public editar(
    id: string,
    veiculoModel: EditarClienteModel,
  ): Observable<EditarClienteResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.put<EditarClienteResponseModel>(urlCompleto, veiculoModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarPorId(id: string): Observable<DetalhesClienteModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.get<DetalhesClienteModel>(urlCompleto);
  }

  public selecionarTodos(): Observable<SelecionarClientesModel[]> {
    return this.http
      .get<SelecionarClientesResponseModel>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }
}
