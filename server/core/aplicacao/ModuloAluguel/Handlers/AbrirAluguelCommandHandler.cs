using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Handlers;


public sealed class AbrirAluguelCommandHandler(
    AppDbContext appDbContext,
    RepositorioAluguelEmOrm repositorioAluguel,
    ILogger<AbrirAluguelCommandHandler> logger
) : IRequestHandler<AbrirAluguelCommand, Result<AbrirAluguelResult>>
{
    public async Task<Result<AbrirAluguelResult>> Handle(
        AbrirAluguelCommand command,
        CancellationToken cancellationToken
    )
    {
        Aluguel? registroEncontrado = await repositorioAluguel.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        if (registroEncontrado.Status == StatusAluguel.Concluido)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(
                "O aluguel já foi concluído e não pode ser aberto novamente."
            ));

        try
        {
            registroEncontrado.Abrir();

            await appDbContext.SaveChangesAsync(cancellationToken);

            AbrirAluguelResult result = new();

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a abertura de {@Command}.", command);

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}