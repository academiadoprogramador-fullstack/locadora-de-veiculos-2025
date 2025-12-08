using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;

public record SelecionarGrupoVeiculosPorIdQuery(Guid Id) : IRequest<Result<SelecionarGrupoVeiculosPorIdResult>>;

public record SelecionarGrupoVeiculosPorIdResult(Guid Id, string Nome);