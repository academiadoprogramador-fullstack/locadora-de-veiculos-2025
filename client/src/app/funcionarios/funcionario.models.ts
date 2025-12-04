export interface CadastrarFuncionarioModel {
  nomeCompleto: string;
  cpf: string;
  email: string;
  senha: string;
  confirmarSenha: string;
  salario: number;
  admissaoEmUtc: Date;
}

export interface CadastrarFuncionarioResponseModel {
  id: string;
}

export interface EditarFuncionarioModel {
  nomeCompleto: string;
  cpf: string;
  salario: number;
  admissaoEmUtc: Date;
}

export interface EditarFuncionarioResponseModel {
  nomeCompleto: string;
  cpf: string;
  salario: number;
  admissaoEmUtc: Date;
}

export interface SelecionarFuncionariosResponseModel {
  registros: SelecionarFuncionariosModel[];
}

export interface SelecionarFuncionariosModel {
  id: string;
  nomeCompleto: string;
  email: string;
  salario: number;
  admissaoEmUtc: string;
}

export interface DetalhesFuncionarioModel {
  id: string;
  nomeCompleto: string;
  cpf: string;
  email: string;
  salario: number;
  admissaoEmUtc: string;
}
