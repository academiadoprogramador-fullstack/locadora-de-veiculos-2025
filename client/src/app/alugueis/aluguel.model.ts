import { TipoCobrancaTaxa } from '../taxas/taxa.models';
import { TipoCombustivel } from '../veiculos/veiculo.models';

export enum MarcadorCombustivel {
  Vazio = 'Vazio',
  UmQuarto = 'UmQuarto',
  MeioTanque = 'MeioTanque',
  TresQuartos = 'TresQuartos',
  Completo = 'Completo',
}

export enum TipoPlanoCobranca {
  Diario = 'Diario',
  Controlado = 'Controlado',
  Livre = 'Livre',
}

export enum StatusAluguel {
  Simulacao = 'Simulacao',
  Aberto = 'Aberto',
  Concluido = 'Concluido',
}

export interface SimilarAberturaAluguelModel {
  condutorId: string;
  veiculoId: string;
  configuracaoCombustiveisId: string;
  tipoPlano: TipoPlanoCobranca;
  inicioEmUtc: Date;
  devolucaoPrevistaEmUtc: Date;
  taxasSelecionadasIds?: string[];
}

export interface SimularAberturaAluguelResponseModel {
  id: string;
  quantidadeDias: number;
  valorPlano: number;
  valorTaxas: number;
  valorParcial: number;
}

export interface EditarAluguelModel {
  condutorId: string;
  veiculoId: string;
  tipoPlano: TipoPlanoCobranca;
  inicioEmUtc: Date;
  devolucaoPrevistaEmUtc: Date;
  taxasSelecionadasIds?: string[];
}

export interface EditarAluguelResponseModel {
  condutorId: string;
  veiculoId: string;
  configuracaoCombustiveisId: string;
  tipoPlano: TipoPlanoCobranca;
  dataInicio: string;
  devolucaoPrevista: string;
  taxasSelecionadasIds: string[];
}

export interface SelecionarAlugueisResponseModel {
  registros: SelecionarAlugueisModel[];
}

export interface SelecionarAlugueisModel {
  id: string;

  condutorId: string;
  condutorNome: string;

  veiculoId: string;
  veiculoDescricao: string;

  configuracaoCombustiveisId: string;

  tipoPlano: TipoPlanoCobranca;
  status: StatusAluguel;

  inicioEmUtc: string;
  devolucaoPrevistaEmUtc: string;
}

export interface DetalhesAluguelModel {
  id: string;

  condutor: CondutorDto;
  veiculo: VeiculoDto;
  configuracaoCombustiveis: ConfiguracaoCombustiveisDto;

  tipoPlano: TipoPlanoCobranca;
  status: StatusAluguel;

  inicioEmUtc: string;
  devolucaoPrevistaEmUtc: string;

  devolucao?: DevolucaoDto;

  taxasSelecionadas: TaxaDto[];
}

export interface SelecionarAlugueisModel {
  id: string;

  condutorId: string;
  condutorNome: string;
  veiculoId: string;
  veiculoDescricao: string;
  configuracaoCombustiveisId: string;

  tipoPlano: TipoPlanoCobranca;

  status: StatusAluguel;

  inicioEmUtc: string;
  devolucaoPrevistaEmUtc: string;
  devolucaoEmUtc?: string;

  taxasSelecionadasIds?: string[];
}

export interface DetalhesAluguelModel {
  id: string;

  condutor: CondutorDto;
  veiculo: VeiculoDto;
  configuracaoCombustiveis: ConfiguracaoCombustiveisDto;

  tipoPlano: TipoPlanoCobranca;

  status: StatusAluguel;

  inicioEmUtc: string;
  devolucaoPrevistaEmUtc: string;
  devolucaoEmUtc?: string;

  taxasSelecionadas: TaxaDto[];
}

export interface CondutorDto {
  id: string;
  nome: string;
}

export interface VeiculoDto {
  id: string;
  marca: string;
  modelo: string;
  tipoCombustivel: TipoCombustivel;
}

export interface ConfiguracaoCombustiveisDto {
  id: string;
  criadaEm: string;
  valorAlcool: number;
  valorDiesel: number;
  valorEletricidade: number;
  valorGas: number;
  valorGasolina: number;
}

export interface TaxaDto {
  id: string;
  nome: string;
  valor: number;
  tipoCobranca: TipoCobrancaTaxa;
}

export interface DevolucaoDto {
  id: string;
  ocorrenciaEmUtc: string;
  marcadorCombustivel: MarcadorCombustivel;
  quilometragemPercorrida: number;
}

// ---- Simulações ----

export interface SimularAberturaAluguelResponseModel {
  aluguelId: string;
  diasCobrados: number;
  valorPlano: number;
  valorTaxas: number;
  valorParcial: number;
  taxasConsideradasIds: string[];
}

export interface SimularConclusaoAluguelModel {
  quilometragemPercorrida: number;
  marcadorCombustivel: MarcadorCombustivel;
  taxasAdicionaisIds?: string[];
}

export interface SimularConclusaoAluguelResponseModel {
  id: string;
  quantidadeDias: number;
  valorPlano: number;
  valorTaxas: number;
  valorParcial: number;
  totalAbastecimento: number;
  valorMulta: number;
  valorTotal: number;
}

// ---- Abertura / Conclusão ----
export interface ConcluirAluguelModel {
  marcadorCombustivel: MarcadorCombustivel;
  quilometragemPercorrida: number;
  taxasAdicionaisIds?: string[];
}

export interface ConcluirAluguelResponseModel {
  id: string;
  concluido: true;
  dataDevolucao: string;
}
