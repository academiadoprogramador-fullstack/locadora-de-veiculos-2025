export interface CadastrarConfiguracaoCombustiveisModel {
  valorAlcool: number;
  valorDiesel: number;
  valorEletricidade: number;
  valorGas: number;
  valorGasolina: number;
}

export interface CadastrarConfiguracaoCombustiveisResponseModel {
  id: string;
}

export interface SelecionarConfiguracoesCombustiveisResponseModel {
  registros: SelecionarConfiguracoesCombustiveisModel[];
}

export interface SelecionarConfiguracoesCombustiveisModel {
  id: string;
  criadaEm: string;
  valorAlcool: number;
  valorDiesel: number;
  valorEletricidade: number;
  valorGas: number;
  valorGasolina: number;
}

export interface SelecionarUltimaConfiguracaoCombustiveisResponseModel {
  id: string;
  criadaEm: string;
  valorAlcool: number;
  valorDiesel: number;
  valorEletricidade: number;
  valorGas: number;
  valorGasolina: number;
}

export interface DetalhesConfiguracaoCombustiveisModel {
  id: string;
  criadaEm: string;
  valorAlcool: number;
  valorDiesel: number;
  valorEletricidade: number;
  valorGas: number;
  valorGasolina: number;
}
