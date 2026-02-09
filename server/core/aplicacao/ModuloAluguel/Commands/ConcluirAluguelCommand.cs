using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

public record ConcluirAluguelCommand(
    Guid Id,
    int QuilometragemPercorrida,
    MarcadorCombustivel MarcadorCombustivel,
    IReadOnlyList<Guid>? TaxasAdicionaisIds
) : IRequest<Result<ConcluirAluguelResult>>;

public record ConcluirAluguelResult(
    Guid Id,
    DateTimeOffset DevolucaoEmUtc
);