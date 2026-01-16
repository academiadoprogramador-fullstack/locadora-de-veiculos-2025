export interface PlanoDiarioDto {
  precoDiarioPlanoDiario: number;
  precoQuilometroPlanoDiario: number;
}

export interface PlanoControladoDto {
  precoDiarioPlanoControlado: number;
  quilometrosDisponiveisPlanoControlado: number;
  precoQuilometroExtrapoladoPlanoControlado: number;
}

export interface PlanoLivreDto {
  precoDiarioPlanoLivre: number;
}

export interface CadastrarPlanoCobrancaModel {
  grupoVeiculosId: string;
  precosPlanoDiario: PlanoDiarioDto;
  precosPlanoControlado: PlanoControladoDto;
  precosPlanoLivre: PlanoLivreDto;
}

export interface CadastrarPlanoCobrancaResponseModel {
  id: string;
}

export interface EditarPlanoCobrancaModel {
  precosPlanoDiario: PlanoDiarioDto;
  precosPlanoControlado: PlanoControladoDto;
  precosPlanoLivre: PlanoLivreDto;
}

export interface EditarPlanoCobrancaResponseModel {
  precosPlanoDiario: PlanoDiarioDto;
  precosPlanoControlado: PlanoControladoDto;
  precosPlanoLivre: PlanoLivreDto;
}

export interface SelecionarPlanosCobrancaResponseModel {
  registros: SelecionarPlanosCobrancaModel[];
}

export interface SelecionarPlanosCobrancaModel {
  id: string;
  grupoVeiculos: GrupoVeiculosDto;
  precosPlanoDiario: PlanoDiarioDto;
  precosPlanoControlado: PlanoControladoDto;
  precosPlanoLivre: PlanoLivreDto;
}

export interface DetalhesPlanoCobrancaModel {
  id: string;
  grupoVeiculos: GrupoVeiculosDto;
  precosPlanoDiario: PlanoDiarioDto;
  precosPlanoControlado: PlanoControladoDto;
  precosPlanoLivre: PlanoLivreDto;
}

export interface GrupoVeiculosDto {
  id: string;
  nome: string;
}
