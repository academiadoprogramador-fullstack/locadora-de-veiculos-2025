using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;

public record EditarGrupoVeiculosCommand(Guid Id, string Nome) : IRequest<Result<EditarGrupoVeiculosResult>>;

public record EditarGrupoVeiculosResult(string Nome);