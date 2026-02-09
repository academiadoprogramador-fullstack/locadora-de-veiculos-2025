import { map, Observable } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import {
    ConcluirAluguelModel, ConcluirAluguelResponseModel, DetalhesAluguelModel, EditarAluguelModel,
    SelecionarAlugueisModel, SelecionarAlugueisResponseModel, SimilarAberturaAluguelModel,
    SimularAberturaAluguelResponseModel, SimularConclusaoAluguelModel,
    SimularConclusaoAluguelResponseModel
} from './aluguel.model';

@Injectable()
export class AluguelService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/alugueis';

  public simularAbertura(
    model: SimilarAberturaAluguelModel,
  ): Observable<SimularAberturaAluguelResponseModel> {
    const urlCompleto = `${this.apiUrl}/simular-abertura`;

    return this.http.post<SimularAberturaAluguelResponseModel>(urlCompleto, model);
  }

  public simularConclusao(
    id: string,
    model: SimularConclusaoAluguelModel,
  ): Observable<SimularConclusaoAluguelResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}/simular-conclusao`;

    return this.http.post<SimularConclusaoAluguelResponseModel>(urlCompleto, model);
  }

  public abrir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}/abrir`;

    return this.http.post<null>(urlCompleto, {});
  }

  public concluir(
    id: string,
    model: ConcluirAluguelModel,
  ): Observable<ConcluirAluguelResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}/concluir`;

    return this.http.post<ConcluirAluguelResponseModel>(urlCompleto, model);
  }

  public editar(
    id: string,
    model: EditarAluguelModel,
  ): Observable<SimularAberturaAluguelResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.put<SimularAberturaAluguelResponseModel>(urlCompleto, model);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarPorId(id: string): Observable<DetalhesAluguelModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.get<DetalhesAluguelModel>(urlCompleto);
  }

  public selecionarTodos(): Observable<SelecionarAlugueisModel[]> {
    return this.http
      .get<SelecionarAlugueisResponseModel>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }
}
