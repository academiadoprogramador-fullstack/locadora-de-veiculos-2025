using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;

public record CadastrarTaxaCommand(
    string Nome,
    decimal Valor,
    TipoCobrancaTaxa TipoCobranca
) : IRequest<Result<CadastrarTaxaResult>>;

public record CadastrarTaxaResult(Guid Id);
