using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculo;
using LocadoraDeVeiculos.Infraestrutura.S3.Repositorios;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Handlers;

public class EditarVeiculoCommandHandler(
    AppDbContext appDbContext,
    RepositorioVeiculoEmOrm repositorioVeiculo,
    RepositorioR2FileStorage fileStorageService,
    ITenantProvider tenantProvider,
    IValidator<EditarVeiculoCommand> validator,
    ILogger<EditarVeiculoCommandHandler> logger
) : IRequestHandler<EditarVeiculoCommand, Result<EditarVeiculoResult>>
{
    public async Task<Result<EditarVeiculoResult>> Handle(
        EditarVeiculoCommand command,
        CancellationToken cancellationToken
    )
    {
        ValidationResult resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        Veiculo? registroEncontrado = await repositorioVeiculo.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            string? imagemKey = null;

            if (command.Imagem is { Length: > 0 })
            {
                var empresaId = tenantProvider.EmpresaId
                    ?? throw new InvalidOperationException("Empresa não identificada.");

                var extensao = Path.GetExtension(command.Imagem.FileName);

                if (string.IsNullOrWhiteSpace(extensao))
                    extensao = ".jpg";

                // Ex: veiculos/{empresaId}/{guid}.jpg
                var key = $"veiculos/{empresaId}/{Guid.NewGuid()}{extensao}";

                await using var stream = command.Imagem.OpenReadStream();

                imagemKey = await fileStorageService.UploadAsync(
                    stream,
                    command.Imagem.ContentType,
                    key,
                    cancellationToken
                );

                if (!string.IsNullOrWhiteSpace(registroEncontrado.Imagem))
                    await fileStorageService.DeleteAsync(registroEncontrado.Imagem, cancellationToken);
            }

            Veiculo veiculoEditado = new(
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.GrupoVeiculosId,
                command.Modelo,
                command.Marca,
                command.Ano,
                imagemKey ?? registroEncontrado.Imagem,
                command.CapacidadeTanque,
                command.TipoCombustivel
            );

            await repositorioVeiculo.EditarAsync(command.Id, veiculoEditado);

            await appDbContext.SaveChangesAsync(cancellationToken);

            EditarVeiculoResult result = new(
                veiculoEditado.GrupoVeiculosId,
                veiculoEditado.Modelo,
                veiculoEditado.Marca,
                veiculoEditado.Ano,
                imagemKey,
                command.CapacidadeTanque,
                command.TipoCombustivel
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Command}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
