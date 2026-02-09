using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

public record ExcluirAluguelCommand(Guid Id) : IRequest<Result<ExcluirAluguelResult>>;

public record ExcluirAluguelResult();