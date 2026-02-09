using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;

public record EditarTaxaCommand(
    Guid Id,
    string Nome,
    decimal Valor,
    TipoCobrancaTaxa TipoCobranca
) : IRequest<Result<EditarTaxaResult>>;

public record EditarTaxaResult(
    string Nome,
    decimal Valor,
    TipoCobrancaTaxa TipoCobranca
);