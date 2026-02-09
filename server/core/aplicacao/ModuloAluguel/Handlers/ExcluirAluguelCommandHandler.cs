using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Handlers;

public sealed class ExcluirAluguelCommandHandler(
    AppDbContext appDbContext,
    RepositorioAluguelEmOrm repositorioAluguel,
    ILogger<ExcluirAluguelCommandHandler> logger
) : IRequestHandler<ExcluirAluguelCommand, Result<ExcluirAluguelResult>>
{
    public async Task<Result<ExcluirAluguelResult>> Handle(
        ExcluirAluguelCommand command,
        CancellationToken cancellationToken
    )
    {
        Aluguel? registroEncontrado = await repositorioAluguel.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            await repositorioAluguel.ExcluirAsync(command.Id);

            await appDbContext.SaveChangesAsync(cancellationToken);

            ExcluirAluguelResult result = new();

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a exclusão de {@Command}.", command);

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
