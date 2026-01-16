using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculo;
using LocadoraDeVeiculos.Infraestrutura.S3.Repositorios;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Handlers;

public class ExcluirVeiculoCommandHandler(
    AppDbContext appDbContext,
    RepositorioVeiculoEmOrm repositorioVeiculo,
    RepositorioR2FileStorage fileStorageService,
    ILogger<ExcluirVeiculoCommandHandler> logger
) : IRequestHandler<ExcluirVeiculoCommand, Result<ExcluirVeiculoResult>>
{
    public async Task<Result<ExcluirVeiculoResult>> Handle(ExcluirVeiculoCommand command, CancellationToken cancellationToken)
    {
        Veiculo? registroEncontrado = await repositorioVeiculo.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            if (!string.IsNullOrWhiteSpace(registroEncontrado.Imagem))
                await fileStorageService.DeleteAsync(registroEncontrado.Imagem, cancellationToken);

            await repositorioVeiculo.ExcluirAsync(command.Id);

            await appDbContext.SaveChangesAsync(cancellationToken);

            ExcluirVeiculoResult result = new();

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