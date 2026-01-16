using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloPlanoCobranca;

public record EditarPlanoCobrancaRequest(
    PlanoDiarioDto PrecosPlanoDiario,
    PlanoControladoDto PrecosPlanoControlado,
    PlanoLivreDto PrecosPlanoLivre
);

public record EditarPlanoCobrancaResponse(
    PlanoDiarioDto PrecosPlanoDiario,
    PlanoControladoDto PrecosPlanoControlado,
    PlanoLivreDto PrecosPlanoLivre
);
