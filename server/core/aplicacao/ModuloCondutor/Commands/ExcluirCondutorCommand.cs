using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;

public record ExcluirCondutorCommand(Guid Id) : IRequest<Result<ExcluirCondutorResult>>;

public record ExcluirCondutorResult();





