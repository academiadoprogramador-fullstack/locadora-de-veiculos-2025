using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxa;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Handlers;

public sealed class SelecionarTaxasQueryHandler(
    RepositorioTaxaEmOrm repositorioTaxa
) : IRequestHandler<SelecionarTaxasQuery, Result<SelecionarTaxasResult>>
{
    public async Task<Result<SelecionarTaxasResult>> Handle(
        SelecionarTaxasQuery request,
        CancellationToken cancellationToken
    )
    {
        var registros = await repositorioTaxa.SelecionarTodosAsync();

        var dtos = registros
            .Select(r => new SelecionarTaxasDto(
                r.Id,
                r.Nome,
                r.Valor,
                r.TipoCobranca
            ))
            .ToImmutableList();

        var response = new SelecionarTaxasResult(dtos);

        return Result.Ok(response);
    }
}