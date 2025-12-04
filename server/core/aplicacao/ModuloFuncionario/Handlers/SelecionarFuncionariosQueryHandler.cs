using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloFuncionario;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Handlers;

public class SelecionarFuncionariosQueryHandler(
    RepositorioFuncionarioEmOrm repositorioFuncionario
) : IRequestHandler<SelecionarFuncionariosQuery, Result<SelecionarFuncionariosResult>>
{
    public async Task<Result<SelecionarFuncionariosResult>> Handle(
        SelecionarFuncionariosQuery query, CancellationToken cancellationToken)
    {
        var registros = await repositorioFuncionario.SelecionarTodosAsync();

        var dtos = registros
            .Select(r => new SelecionarFuncionariosDto(
                r.Id,
                r.NomeCompleto,
                r.Email,
                r.Salario,
                r.AdmissaoEmUtc
            ))
            .ToImmutableList();

        var response = new SelecionarFuncionariosResult(dtos);

        return Result.Ok(response);
    }
}
