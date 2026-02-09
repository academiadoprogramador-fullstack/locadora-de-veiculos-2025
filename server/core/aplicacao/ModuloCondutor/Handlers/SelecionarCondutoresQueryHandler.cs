using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCondutor;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Handlers;

public sealed class SelecionarCondutoresQueryHandler(
    RepositorioCondutorEmOrm repositorioCondutor
) : IRequestHandler<SelecionarCondutoresQuery, Result<SelecionarCondutoresResult>>
{
    public async Task<Result<SelecionarCondutoresResult>> Handle(
        SelecionarCondutoresQuery request,
        CancellationToken cancellationToken
    )
    {
        var registros = await repositorioCondutor.SelecionarTodosAsync();

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

        var response = new SelecionarCondutoresResult(dtos);

        return Result.Ok(response);
    }
}