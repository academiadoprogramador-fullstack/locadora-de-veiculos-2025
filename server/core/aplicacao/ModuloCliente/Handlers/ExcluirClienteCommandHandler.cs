using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Handlers;

public sealed class ExcluirClienteCommandHandler(
    AppDbContext appDbContext,
    RepositorioClienteEmOrm repositorioCliente,
    ILogger<ExcluirClienteCommandHandler> logger
) : IRequestHandler<ExcluirClienteCommand, Result<ExcluirClienteResult>>
{
    public async Task<Result<ExcluirClienteResult>> Handle(
        ExcluirClienteCommand command,
        CancellationToken cancellationToken
    )
    {
        Cliente? registroEncontrado = await repositorioCliente.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            await repositorioCliente.ExcluirAsync(command.Id);

            await appDbContext.SaveChangesAsync(cancellationToken);

            ExcluirClienteResult result = new();

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
