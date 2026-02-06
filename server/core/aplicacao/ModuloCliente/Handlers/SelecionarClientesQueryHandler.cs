using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Handlers;

public sealed class SelecionarClientesQueryHandler(
    RepositorioClienteEmOrm repositorioCliente
) : IRequestHandler<SelecionarClientesQuery, Result<SelecionarClientesResult>>
{
    public async Task<Result<SelecionarClientesResult>> Handle(
        SelecionarClientesQuery request,
        CancellationToken cancellationToken
    )
    {
        var registros = await repositorioCliente.SelecionarTodosAsync();

        var dtos = registros
            .Select(r => new SelecionarClientesDto(
                r.Id,
                r.Nome,
                r.Email,
                r.Telefone,
                r.Tipo,
                r.NumeroDocumento,
                r.Cidade,
                r.Estado,
                r.Bairro,
                r.Rua,
                r.Numero
            ))
            .ToImmutableList();

        var response = new SelecionarClientesResult(dtos);

        return Result.Ok(response);
    }
}
