using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloAluguel;

public sealed record EditarAluguelRequest(
    Guid CondutorId,
    Guid VeiculoId,
    TipoPlanoCobranca TipoPlano,
    DateTimeOffset InicioEmUtc,
    DateTimeOffset DevolucaoPrevistaEmUtc,
    IReadOnlyList<Guid> TaxasSelecionadasIds
);
