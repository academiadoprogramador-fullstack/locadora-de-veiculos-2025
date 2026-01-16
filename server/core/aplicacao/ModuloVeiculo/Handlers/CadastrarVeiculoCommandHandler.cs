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

public sealed class CadastrarVeiculoCommandHandler(
    AppDbContext appDbContext,
    RepositorioVeiculoEmOrm repositorioVeiculo,
    RepositorioR2FileStorage fileStorageService,
    ITenantProvider tenantProvider,
    IValidator<CadastrarVeiculoCommand> validator,
    ILogger<CadastrarVeiculoCommandHandler> logger
) : IRequestHandler<CadastrarVeiculoCommand, Result<CadastrarVeiculoResult>>
{
    public async Task<Result<CadastrarVeiculoResult>> Handle(
        CadastrarVeiculoCommand command,
        CancellationToken cancellationToken
    )
    {
        ValidationResult resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        try
        {
            string imagemKey = string.Empty;

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
            }

            Veiculo veiculo = new(
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.GrupoVeiculosId,
                command.Modelo,
                command.Marca,
                command.Ano,
                imagemKey,
                command.CapacidadeTanque,
                command.TipoCombustivel
            );

            await repositorioVeiculo.CadastrarAsync(veiculo);

            await appDbContext.SaveChangesAsync(cancellationToken);

            CadastrarVeiculoResult result = new(veiculo.Id);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante o cadastro de {@Command}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
