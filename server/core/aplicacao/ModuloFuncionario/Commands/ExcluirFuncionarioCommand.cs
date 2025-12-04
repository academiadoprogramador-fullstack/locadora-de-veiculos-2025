using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands;

public record ExcluirFuncionarioCommand(Guid Id) : IRequest<Result<ExcluirFuncionarioResult>>;

public record ExcluirFuncionarioResult();