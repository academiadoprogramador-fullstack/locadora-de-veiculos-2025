import { map, Observable } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import {
  CadastrarCondutorModel,
  CadastrarCondutorResponseModel,
  DetalhesCondutorModel,
  EditarCondutorModel,
  EditarCondutorResponseModel,
  SelecionarCondutoresModel,
  SelecionarCondutoresPorClienteResponseModel,
  SelecionarCondutoresResponseModel,
} from './condutor.models';

@Injectable()
export class CondutorService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/condutores';

  public cadastrar(
    veiculoModel: CadastrarCondutorModel,
  ): Observable<CadastrarCondutorResponseModel> {
    return this.http.post<CadastrarCondutorResponseModel>(this.apiUrl, veiculoModel);
  }

  public editar(
    id: string,
    veiculoModel: EditarCondutorModel,
  ): Observable<EditarCondutorResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.put<EditarCondutorResponseModel>(urlCompleto, veiculoModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarPorId(id: string): Observable<DetalhesCondutorModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.get<DetalhesCondutorModel>(urlCompleto);
  }

  public selecionarTodos(): Observable<SelecionarCondutoresModel[]> {
    return this.http
      .get<SelecionarCondutoresResponseModel>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }

  public selecionarCondutoresPorIdCliente(
    clienteId: string,
  ): Observable<SelecionarCondutoresModel[]> {
    const urlCompleto = `${environment.apiUrl}/condutores/cliente/${clienteId}`;

    return this.http
      .get<SelecionarCondutoresPorClienteResponseModel>(urlCompleto)
      .pipe(map((res) => res.registros));
  }
}
