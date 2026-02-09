using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;

public record ExcluirTaxaCommand(Guid Id) : IRequest<Result<ExcluirTaxaResult>>;

public record ExcluirTaxaResult();
