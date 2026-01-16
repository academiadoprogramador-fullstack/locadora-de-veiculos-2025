import { map, Observable } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import {
  CadastrarVeiculoModel,
  CadastrarVeiculoResponseModel,
  DetalhesVeiculoModel,
  EditarVeiculoModel,
  EditarVeiculoResponseModel,
  SelecionarVeiculosModel,
  SelecionarVeiculosResponseModel,
} from './veiculo.models';

@Injectable()
export class VeiculoService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/veiculos';

  public cadastrar(veiculoModel: CadastrarVeiculoModel): Observable<CadastrarVeiculoResponseModel> {
    const formData = new FormData();

    Object.entries(veiculoModel).forEach(([key, value]) => {
      if (value === null || value === undefined) {
        return;
      }

      if (value instanceof File) {
        formData.append(key, value);
      } else {
        formData.append(key, value.toString());
      }
    });

    return this.http.post<CadastrarVeiculoResponseModel>(this.apiUrl, formData);
  }

  public editar(
    id: string,
    veiculoModel: EditarVeiculoModel,
  ): Observable<EditarVeiculoResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    const formData = new FormData();

    Object.entries(veiculoModel).forEach(([key, value]) => {
      if (value === null || value === undefined) {
        return;
      }

      if (value instanceof File) {
        formData.append(key, value);
      } else {
        formData.append(key, value.toString());
      }
    });

    return this.http.put<EditarVeiculoResponseModel>(urlCompleto, formData);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarPorId(id: string): Observable<DetalhesVeiculoModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.get<DetalhesVeiculoModel>(urlCompleto);
  }

  public selecionarTodos(): Observable<SelecionarVeiculosModel[]> {
    return this.http
      .get<SelecionarVeiculosResponseModel>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }
}
