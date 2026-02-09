export interface CadastrarCondutorModel {
  clienteId: string;
  clienteCondutor: boolean;
  nome: string;
  email: string;
  telefone: string;
  cpf: string;
  cnh: string;
  validadeCnh: Date;
}

export interface CadastrarCondutorResponseModel {
  id: string;
}

export interface EditarCondutorModel {
  clienteId: string;
  clienteCondutor: boolean;
  nome: string;
  email: string;
  telefone: string;
  cpf: string;
  cnh: string;
  validadeCnh: Date;
}

export interface EditarCondutorResponseModel {
  clienteId: string;
  clienteCondutor: boolean;
  nome: string;
  email: string;
  telefone: string;
  cpf: string;
  cnh: string;
  validadeCnh: string;
}

export interface SelecionarCondutoresResponseModel {
  registros: SelecionarCondutoresModel[];
}

export interface SelecionarCondutoresModel {
  id: string;
  clienteId: string;
  clienteCondutor: boolean;
  nome: string;
  email: string;
  telefone: string;
  cpf: string;
  cnh: string;
  validadeCnh: string;
}

export interface DetalhesCondutorModel {
  id: string;
  cliente: SelecionarClienteDto;
  clienteCondutor: boolean;
  nome: string;
  email: string;
  telefone: string;
  cpf: string;
  cnh: string;
  validadeCnh: string;
}

export interface SelecionarClienteDto {
  id: string;
  nome: string;
}

export interface SelecionarCondutoresPorClienteResponseModel {
  registros: SelecionarCondutoresModel[];
}
