export enum TipoCliente {
  Cpf = 'Cpf',
  Cnpj = 'Cnpj',
}

export interface CadastrarClienteModel {
  nome: string;
  email: string;
  telefone: string;
  tipo: TipoCliente;
  numeroDocumento: string;
  cidade: string;
  estado: string;
  bairro: string;
  rua: string;
  numero: string;
}

export interface CadastrarClienteResponseModel {
  id: string;
}

export interface EditarClienteModel {
  nome: string;
  email: string;
  telefone: string;
  tipo: TipoCliente;
  numeroDocumento: string;
  cidade: string;
  estado: string;
  bairro: string;
  rua: string;
  numero: string;
}

export interface EditarClienteResponseModel {
  nome: string;
  email: string;
  telefone: string;
  tipo: TipoCliente;
  numeroDocumento: string;
  cidade: string;
  estado: string;
  bairro: string;
  rua: string;
  numero: string;
}

export interface SelecionarClientesResponseModel {
  registros: SelecionarClientesModel[];
}

export interface SelecionarClientesModel {
  id: string;
  nome: string;
  email: string;
  telefone: string;
  tipo: TipoCliente;
  numeroDocumento: string;
  cidade: string;
  estado: string;
  bairro: string;
  rua: string;
  numero: string;
}

export interface DetalhesClienteModel {
  id: string;
  nome: string;
  email: string;
  telefone: string;
  tipo: TipoCliente;
  numeroDocumento: string;
  cidade: string;
  estado: string;
  bairro: string;
  rua: string;
  numero: string;
}
