using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Handlers;

public class ExcluirPlanoCobrancaCommandHandler(
    AppDbContext appDbContext,
    RepositorioPlanoCobrancaEmOrm repositorioPlanoCobranca,
    ILogger<ExcluirPlanoCobrancaCommandHandler> logger
) : IRequestHandler<ExcluirPlanoCobrancaCommand, Result<ExcluirPlanoCobrancaResult>>
{
    public async Task<Result<ExcluirPlanoCobrancaResult>> Handle(
        ExcluirPlanoCobrancaCommand command,
        CancellationToken cancellationToken
    )
    {
        PlanoCobranca? registroEncontrado = await repositorioPlanoCobranca.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            await repositorioPlanoCobranca.ExcluirAsync(command.Id);

            await appDbContext.SaveChangesAsync(cancellationToken);

            ExcluirPlanoCobrancaResult result = new();

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
