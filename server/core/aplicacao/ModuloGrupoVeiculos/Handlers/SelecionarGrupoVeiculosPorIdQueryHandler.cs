using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Handlers;

public class SelecionarGrupoVeiculosPorIdQueryHandler(RepositorioGrupoVeiculosEmOrm repositorioGrupoVeiculos)
    : IRequestHandler<SelecionarGrupoVeiculosPorIdQuery, Result<SelecionarGrupoVeiculosPorIdResult>>
{
    public async Task<Result<SelecionarGrupoVeiculosPorIdResult>> Handle(
        SelecionarGrupoVeiculosPorIdQuery query, CancellationToken cancellationToken)
    {
        GrupoVeiculos? registroEncontrado = await repositorioGrupoVeiculos.SelecionarPorIdAsync(query.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

        SelecionarGrupoVeiculosPorIdResult response = new(
            query.Id,
            registroEncontrado.Nome
        );

        return Result.Ok(response);
    }
}
