using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxa;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Handlers;

public sealed class ExcluirTaxaCommandHandler(
    AppDbContext appDbContext,
    RepositorioTaxaEmOrm repositorioTaxa,
    ILogger<ExcluirTaxaCommandHandler> logger
) : IRequestHandler<ExcluirTaxaCommand, Result<ExcluirTaxaResult>>
{
    public async Task<Result<ExcluirTaxaResult>> Handle(
        ExcluirTaxaCommand command,
        CancellationToken cancellationToken
    )
    {
        Taxa? registroEncontrado = await repositorioTaxa.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            await repositorioTaxa.ExcluirAsync(command.Id);

            await appDbContext.SaveChangesAsync(cancellationToken);

            ExcluirTaxaResult result = new();

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a exclusão de {@Command}.", command);
            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
