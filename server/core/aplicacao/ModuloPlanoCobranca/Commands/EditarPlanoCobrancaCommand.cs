using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;

public record EditarPlanoCobrancaCommand(
    Guid Id,
    PlanoDiarioDto PrecosPlanoDiario,
    PlanoControladoDto PrecosPlanoControlado,
    PlanoLivreDto PrecosPlanoLivre
) : IRequest<Result<EditarPlanoCobrancaResult>>;

public record EditarPlanoCobrancaResult(
    PlanoDiarioDto PrecosPlanoDiario,
    PlanoControladoDto PrecosPlanoControlado,
    PlanoLivreDto PrecosPlanoLivre
);