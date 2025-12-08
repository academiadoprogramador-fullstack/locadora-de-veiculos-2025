using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Handlers;

public class ExcluirGrupoVeiculosCommandHandler(
    AppDbContext appDbContext,
    RepositorioGrupoVeiculosEmOrm repositorioGrupoVeiculos,
    ILogger<ExcluirGrupoVeiculosCommandHandler> logger
) : IRequestHandler<ExcluirGrupoVeiculosCommand, Result<ExcluirGrupoVeiculosResult>>
{
    public async Task<Result<ExcluirGrupoVeiculosResult>> Handle(
        ExcluirGrupoVeiculosCommand command, CancellationToken cancellationToken)
    {
        GrupoVeiculos? registroEncontrado = await repositorioGrupoVeiculos.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            await repositorioGrupoVeiculos.ExcluirAsync(command.Id);

            await appDbContext.SaveChangesAsync(cancellationToken);

            ExcluirGrupoVeiculosResult result = new();

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão de {@Command}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
