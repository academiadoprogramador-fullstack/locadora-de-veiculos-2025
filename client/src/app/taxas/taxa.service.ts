import { map, Observable } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import {
  CadastrarTaxaModel,
  CadastrarTaxaResponseModel,
  DetalhesTaxaModel,
  EditarTaxaModel,
  EditarTaxaResponseModel,
  SelecionarTaxasModel,
  SelecionarTaxasResponseModel,
} from './taxa.models';

@Injectable()
export class TaxaService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/taxas';

  public cadastrar(veiculoModel: CadastrarTaxaModel): Observable<CadastrarTaxaResponseModel> {
    return this.http.post<CadastrarTaxaResponseModel>(this.apiUrl, veiculoModel);
  }

  public editar(id: string, veiculoModel: EditarTaxaModel): Observable<EditarTaxaResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.put<EditarTaxaResponseModel>(urlCompleto, veiculoModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarPorId(id: string): Observable<DetalhesTaxaModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.get<DetalhesTaxaModel>(urlCompleto);
  }

  public selecionarTodos(): Observable<SelecionarTaxasModel[]> {
    return this.http
      .get<SelecionarTaxasResponseModel>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }
}
