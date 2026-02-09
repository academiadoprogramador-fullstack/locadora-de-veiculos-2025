using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;

public record SelecionarTaxasQuery()
    : IRequest<Result<SelecionarTaxasResult>>;

public record SelecionarTaxasResult(
    IReadOnlyList<SelecionarTaxasDto> Registros
);

public record SelecionarTaxasDto(
    Guid Id,
    string Nome,
    decimal Valor,
    TipoCobrancaTaxa TipoCobranca
);