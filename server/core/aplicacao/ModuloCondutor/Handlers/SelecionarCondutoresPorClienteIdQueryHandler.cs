using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCondutor;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Handlers;

public sealed class SelecionarCondutoresPorClienteIdQueryHandler(
    RepositorioCondutorEmOrm repositorioCondutor
) : IRequestHandler<SelecionarCondutoresPorClienteIdQuery, Result<SelecionarCondutoresPorClienteIdResult>>
{
    public async Task<Result<SelecionarCondutoresPorClienteIdResult>> Handle(
        SelecionarCondutoresPorClienteIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var registros = await repositorioCondutor.SelecionarCondutoresPorIdClienteAsync(query.ClienteId);

        var dtos = registros
            .Select(r => new SelecionarCondutoresDto(
                r.Id,
                r.ClienteId,
                r.ClienteCondutor,
                r.Nome,
                r.Email,
                r.Telefone,
                r.Cpf,
                r.Cnh,
                r.ValidadeCnh
            ))
            .ToImmutableList();

        var response = new SelecionarCondutoresPorClienteIdResult(dtos);

        return Result.Ok(response);
    }
}