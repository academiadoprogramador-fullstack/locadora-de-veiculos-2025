using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;

public record ExcluirVeiculoCommand(Guid Id) : IRequest<Result<ExcluirVeiculoResult>>;

public record ExcluirVeiculoResult();
