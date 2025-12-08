using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;

public record CadastrarGrupoVeiculosCommand(string Nome) : IRequest<Result<CadastrarGrupoVeiculosResult>>;

public record CadastrarGrupoVeiculosResult(Guid Id);

