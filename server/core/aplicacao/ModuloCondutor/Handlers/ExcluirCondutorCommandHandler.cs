using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCondutor;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Handlers;

public sealed class ExcluirCondutorCommandHandler(
    AppDbContext appDbContext,
    RepositorioCondutorEmOrm repositorioCondutor,
    ILogger<ExcluirCondutorCommandHandler> logger
) : IRequestHandler<ExcluirCondutorCommand, Result<ExcluirCondutorResult>>
{
    public async Task<Result<ExcluirCondutorResult>> Handle(
        ExcluirCondutorCommand command,
        CancellationToken cancellationToken
    )
    {
        Condutor? registroEncontrado = await repositorioCondutor.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            await repositorioCondutor.ExcluirAsync(command.Id);

            await appDbContext.SaveChangesAsync(cancellationToken);

            ExcluirCondutorResult result = new();

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
