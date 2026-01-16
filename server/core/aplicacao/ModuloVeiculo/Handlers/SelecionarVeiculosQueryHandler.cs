using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculo;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Handlers;

public class SelecionarVeiculosQueryHandler(
    RepositorioVeiculoEmOrm repositorioVeiculo
) : IRequestHandler<SelecionarVeiculosQuery, Result<SelecionarVeiculosResult>>
{
    public async Task<Result<SelecionarVeiculosResult>> Handle(
        SelecionarVeiculosQuery request, CancellationToken cancellationToken)
    {
        var registros = await repositorioVeiculo.SelecionarTodosAsync();

        var dtos = registros
            .Select(r => new SelecionarVeiculosDto(
                r.Id,
                r.GrupoVeiculosId,
                r.Modelo,
                r.Marca,
                r.Ano,
                r.Imagem,
                r.CapacidadeTanque,
                r.TipoCombustivel
            ))
            .ToImmutableList();

        var response = new SelecionarVeiculosResult(dtos);

        return Result.Ok(response);
    }
}
