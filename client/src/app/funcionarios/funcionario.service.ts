import { map, Observable } from 'rxjs';

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import {
    CadastrarFuncionarioModel, CadastrarFuncionarioResponseModel, DetalhesFuncionarioModel,
    EditarFuncionarioModel, EditarFuncionarioResponseModel, SelecionarFuncionariosModel,
    SelecionarFuncionariosResponseModel
} from './funcionario.models';

@Injectable()
export class FuncionarioService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl + '/funcionarios';

  public cadastrar(
    funcionarioModel: CadastrarFuncionarioModel,
  ): Observable<CadastrarFuncionarioResponseModel> {
    return this.http.post<CadastrarFuncionarioResponseModel>(this.apiUrl, funcionarioModel);
  }

  public editar(
    id: string,
    funcionarioModel: EditarFuncionarioModel,
  ): Observable<EditarFuncionarioResponseModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.put<EditarFuncionarioResponseModel>(urlCompleto, funcionarioModel);
  }

  public excluir(id: string): Observable<null> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.delete<null>(urlCompleto);
  }

  public selecionarPorId(id: string): Observable<DetalhesFuncionarioModel> {
    const urlCompleto = `${this.apiUrl}/${id}`;

    return this.http.get<DetalhesFuncionarioModel>(urlCompleto);
  }

  public selecionarTodos(): Observable<SelecionarFuncionariosModel[]> {
    return this.http
      .get<SelecionarFuncionariosResponseModel>(this.apiUrl)
      .pipe(map((res) => res.registros));
  }
}
