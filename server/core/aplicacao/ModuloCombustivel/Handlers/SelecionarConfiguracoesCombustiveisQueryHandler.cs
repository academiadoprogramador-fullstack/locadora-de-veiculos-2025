using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCombustivel;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Handlers;

public sealed class SelecionarConfiguracoesCombustiveisQueryHandler(
    RepositorioConfiguracaoCombustiveisEmOrm repositorioConfiguracao
) : IRequestHandler<SelecionarConfiguracoesCombustiveisQuery, Result<SelecionarConfiguracoesCombustiveisResult>>
{
    public async Task<Result<SelecionarConfiguracoesCombustiveisResult>> Handle(
        SelecionarConfiguracoesCombustiveisQuery request,
        CancellationToken cancellationToken
    )
    {
        var registros = await repositorioConfiguracao.SelecionarConfiguracoesEmOrdemDescendente();

        var dtos = registros
            .Select(r => new SelecionarConfiguracoesCombustiveisDto(
                r.Id,
                r.CriadaEm,
                r.ValorAlcool,
                r.ValorDiesel,
                r.ValorEletricidade,
                r.ValorGas,
                r.ValorGasolina
            ))
            .ToImmutableList();

        var response = new SelecionarConfiguracoesCombustiveisResult(dtos);

        return Result.Ok(response);
    }
}
