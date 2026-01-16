using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;

public record SelecionarPlanoCobrancaPorIdQuery(Guid Id)
    : IRequest<Result<SelecionarPlanoCobrancaPorIdResult>>;

public record SelecionarPlanoCobrancaPorIdResult(
    Guid Id,
    SelecionarGruposVeiculosDto GrupoVeiculos,
    PlanoDiarioDto PrecosPlanoDiario,
    PlanoControladoDto PrecosPlanoControlado,
    PlanoLivreDto PrecosPlanoLivre
);