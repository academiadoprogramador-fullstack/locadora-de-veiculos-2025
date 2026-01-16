using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloPlanoCobranca;

public record CadastrarPlanoCobrancaRequest(
    Guid GrupoVeiculosId,
    PlanoDiarioDto PrecosPlanoDiario,
    PlanoControladoDto PrecosPlanoControlado,
    PlanoLivreDto PrecosPlanoLivre
);

public record class CadastrarPlanoCobrancaResponse(Guid Id);
