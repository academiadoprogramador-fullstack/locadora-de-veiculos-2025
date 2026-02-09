using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

public record SimularConclusaoAluguelQuery(
    Guid AluguelId,
    MarcadorCombustivel MarcadorCombustivel,
    int QuilometragemPercorrida,
    IReadOnlyList<Guid>? TaxasAdicionaisIds
) : IRequest<Result<SimularConclusaoAluguelResult>>;

public record SimularConclusaoAluguelResult(
    Guid AluguelId,
    int QuantidadeDias,
    decimal ValorPlano,
    decimal ValorTaxas,
    decimal ValorParcial,
    decimal TotalAbastecimento,
    decimal ValorMulta,
    decimal ValorTotal
);
