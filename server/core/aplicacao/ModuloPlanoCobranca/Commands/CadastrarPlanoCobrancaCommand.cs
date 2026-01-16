using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;

public record CadastrarPlanoCobrancaCommand(
    Guid GrupoVeiculosId,
    PlanoDiarioDto PrecosPlanoDiario,
    PlanoControladoDto PrecosPlanoControlado,
    PlanoLivreDto PrecosPlanoLivre
) : IRequest<Result<CadastrarPlanoCobrancaResult>>;

public record PlanoDiarioDto(
    decimal PrecoDiarioPlanoDiario, 
    decimal PrecoQuilometroPlanoDiario
);

public record PlanoControladoDto(
    decimal QuilometrosDisponiveisPlanoControlado,
    decimal PrecoDiarioPlanoControlado,
    decimal PrecoQuilometroExtrapoladoPlanoControlado
);

public record PlanoLivreDto(decimal PrecoDiarioPlanoLivre);

public record class CadastrarPlanoCobrancaResult(Guid Id);
