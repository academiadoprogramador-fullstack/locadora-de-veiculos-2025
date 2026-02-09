using LocadoraDeVeiculos.Dominio.ModuloVeiculo;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloAluguel;

public record SimularConclusaoAluguelRequest(
    int QuilometragemPercorrida,
    MarcadorCombustivel MarcadorCombustivel,
    IReadOnlyList<Guid>? TaxasAdicionaisIds
);

public record SimularConclusaoAluguelResponse(
    Guid Id,
    int QuantidadeDias,
    decimal ValorPlano,
    decimal ValorTaxas,
    decimal ValorParcial,
    decimal TotalAbastecimento,
    decimal ValorMulta,
    decimal ValorTotal
);
