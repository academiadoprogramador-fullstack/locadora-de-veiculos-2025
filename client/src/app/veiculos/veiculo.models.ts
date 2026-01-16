export enum TipoCombustivel {
  Alcool = 'Alcool',
  Diesel = 'Diesel',
  Gas = 'Gas',
  Gasolina = 'Gasolina',
}

export interface CadastrarVeiculoModel {
  grupoVeiculosId: string;
  modelo: string;
  marca: string;
  ano: number;
  capacidadeTanque: number;
  tipoCombustivel: TipoCombustivel;
  imagem: File;
}

export interface CadastrarVeiculoResponseModel {
  id: string;
}

export interface EditarVeiculoModel {
  grupoVeiculosId: string;
  modelo: string;
  marca: string;
  ano: number;
  capacidadeTanque: number;
  tipoCombustivel: TipoCombustivel;
  imagem: File;
}

export interface EditarVeiculoResponseModel {
  grupoVeiculosId: string;
  modelo: string;
  marca: string;
  ano: number;
  capacidadeTanque: number;
  tipoCombustivel: TipoCombustivel;
  imagem?: string;
}

export interface SelecionarVeiculosResponseModel {
  registros: SelecionarVeiculosModel[];
}

export interface SelecionarVeiculosModel {
  id: string;
  grupoVeiculosId: string;
  modelo: string;
  marca: string;
  ano: number;
  capacidadeTanque: number;
  tipoCombustivel: TipoCombustivel;
  imagem: string;
}

export interface DetalhesVeiculoModel {
  id: string;
  grupoVeiculos: GrupoVeiculosDto;
  modelo: string;
  marca: string;
  ano: number;
  capacidadeTanque: number;
  tipoCombustivel: TipoCombustivel;
  imagem: string;
}

export interface GrupoVeiculosDto {
  id: string;
  nome: string;
}
