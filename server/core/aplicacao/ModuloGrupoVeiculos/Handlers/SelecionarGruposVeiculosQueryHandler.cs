using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Handlers;

public class SelecionarGruposVeiculosQueryHandler(
    RepositorioGrupoVeiculosEmOrm repositorioGrupoVeiculos
) : IRequestHandler<SelecionarGruposVeiculosQuery, Result<SelecionarGruposVeiculosResult>>
{
    public async Task<Result<SelecionarGruposVeiculosResult>> Handle(
        SelecionarGruposVeiculosQuery request, CancellationToken cancellationToken)
    {
        var registros = await repositorioGrupoVeiculos
            .SelecionarTodosAsync();

        var dtos = registros
            .Select(r => new SelecionarGruposVeiculosDto(
                r.Id,
                r.Nome
            ))
            .ToImmutableList();

        var response = new SelecionarGruposVeiculosResult(dtos);

        return Result.Ok(response);
    }
}
