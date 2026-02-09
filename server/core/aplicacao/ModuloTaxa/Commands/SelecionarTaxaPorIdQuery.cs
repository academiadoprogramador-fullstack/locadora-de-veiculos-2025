using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;

public record SelecionarTaxaPorIdQuery(Guid Id)
    : IRequest<Result<SelecionarTaxaPorIdResult>>;

public record SelecionarTaxaPorIdResult(
    Guid Id,
    string Nome,
    decimal Valor,
    TipoCobrancaTaxa TipoCobranca
);
