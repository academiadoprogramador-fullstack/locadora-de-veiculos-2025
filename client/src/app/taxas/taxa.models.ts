export enum TipoCobrancaTaxa {
  Diaria = 'Diaria',
  Fixa = 'Fixa',
}

export interface CadastrarTaxaModel {
  nome: string;
  valor: number;
  tipoCobranca: TipoCobrancaTaxa;
}

export interface CadastrarTaxaResponseModel {
  id: string;
}

export interface EditarTaxaModel {
  nome: string;
  valor: number;
  tipoCobranca: TipoCobrancaTaxa;
}

export interface EditarTaxaResponseModel {
  nome: string;
  valor: number;
  tipoCobranca: TipoCobrancaTaxa;
}

export interface SelecionarTaxasResponseModel {
  registros: SelecionarTaxasModel[];
}

export interface SelecionarTaxasModel {
  id: string;
  nome: string;
  valor: number;
  tipoCobranca: TipoCobrancaTaxa;
}

export interface DetalhesTaxaModel {
  id: string;
  nome: string;
  valor: number;
  tipoCobranca: TipoCobrancaTaxa;
}
