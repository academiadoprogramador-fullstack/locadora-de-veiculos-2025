using LocadoraDeVeiculos.Dominio.ModuloVeiculo;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloAluguel;

public sealed record ConcluirAluguelRequest(
    int QuilometragemPercorrida,
    MarcadorCombustivel MarcadorCombustivel,
    IReadOnlyList<Guid>? TaxasAdicionaisIds
);

public sealed record ConcluirAluguelResponse(
    Guid Id,
    DateTimeOffset DataDevolucao
);