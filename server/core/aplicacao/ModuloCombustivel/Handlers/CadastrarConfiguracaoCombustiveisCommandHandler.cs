using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloCombustivel;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCombustivel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Handlers;

public sealed class CadastrarConfiguracaoCombustiveisCommandHandler(
    AppDbContext appDbContext,
    RepositorioConfiguracaoCombustiveisEmOrm repositorioConfiguracao,
    ITenantProvider tenantProvider,
    ILogger<CadastrarConfiguracaoCombustiveisCommandHandler> logger
) : IRequestHandler<CadastrarConfiguracaoCombustiveisCommand, Result<CadastrarConfiguracaoCombustiveisResult>>
{
    public async Task<Result<CadastrarConfiguracaoCombustiveisResult>> Handle(
        CadastrarConfiguracaoCombustiveisCommand command,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var empresaId = tenantProvider.EmpresaId
                ?? throw new InvalidOperationException("Empresa não identificada.");

            ConfiguracaoCombustiveis configuracao = new(
                empresaId,
                command.ValorAlcool,
                command.ValorDiesel,
                command.ValorEletricidade,
                command.ValorGas,
                command.ValorGasolina
            );

            await repositorioConfiguracao.Cadastrar(configuracao);

            await appDbContext.SaveChangesAsync(cancellationToken);

            CadastrarConfiguracaoCombustiveisResult result = new(configuracao.Id);

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
