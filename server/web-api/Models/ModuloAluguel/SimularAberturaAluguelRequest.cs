using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloAluguel;

public sealed record SimularAberturaAluguelRequest(
    Guid CondutorId,
    Guid VeiculoId,
    Guid ConfiguracaoCombustiveisId,
    TipoPlanoCobranca TipoPlano,
    DateTimeOffset InicioEmUtc,
    DateTimeOffset DevolucaoPrevistaEmUtc,
    IReadOnlyList<Guid> TaxasSelecionadasIds
);

public record SimularAberturaAluguelResponse(
    Guid Id,
    int QuantidadeDias,
    decimal ValorPlano,
    decimal ValorTaxas,
    decimal ValorParcial
);