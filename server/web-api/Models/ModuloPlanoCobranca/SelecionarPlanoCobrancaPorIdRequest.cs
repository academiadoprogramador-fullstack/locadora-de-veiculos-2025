using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloPlanoCobranca;

public record SelecionarPlanoCobrancaPorIdResponse(
    Guid Id,
    SelecionarGruposVeiculosDto GrupoVeiculos,
    PlanoDiarioDto PrecosPlanoDiario,
    PlanoControladoDto PrecosPlanoControlado,
    PlanoLivreDto PrecosPlanoLivre
);