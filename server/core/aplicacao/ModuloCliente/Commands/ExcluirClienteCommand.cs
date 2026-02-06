using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;

public record ExcluirClienteCommand(Guid Id) : IRequest<Result<ExcluirClienteResult>>;

public record ExcluirClienteResult();