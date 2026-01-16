using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;

public record SelecionarPlanosCobrancaQuery() : IRequest<Result<SelecionarPlanosCobrancaResult>>;

public record SelecionarPlanosCobrancaResult(
    IReadOnlyList<SelecionarPlanosCobrancaDto> Registros
);

public record SelecionarPlanosCobrancaDto(
    Guid Id,
    SelecionarGruposVeiculosDto GrupoVeiculos,
    PlanoDiarioDto PrecosPlanoDiario,
    PlanoControladoDto PrecosPlanoControlado,
    PlanoLivreDto PrecosPlanoLivre
);