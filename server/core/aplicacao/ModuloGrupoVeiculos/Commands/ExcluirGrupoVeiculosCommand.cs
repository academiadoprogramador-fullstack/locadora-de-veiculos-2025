using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;

public record ExcluirGrupoVeiculosCommand(Guid Id) : IRequest<Result<ExcluirGrupoVeiculosResult>>;

public record ExcluirGrupoVeiculosResult();