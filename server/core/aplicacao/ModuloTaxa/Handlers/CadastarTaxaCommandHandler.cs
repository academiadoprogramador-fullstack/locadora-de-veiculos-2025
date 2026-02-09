using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxa;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Handlers;

public sealed class CadastrarTaxaCommandHandler(
    AppDbContext appDbContext,
    RepositorioTaxaEmOrm repositorioTaxa,
    ITenantProvider tenantProvider,
    IValidator<CadastrarTaxaCommand> validator,
    ILogger<CadastrarTaxaCommandHandler> logger
) : IRequestHandler<CadastrarTaxaCommand, Result<CadastrarTaxaResult>>
{
    public async Task<Result<CadastrarTaxaResult>> Handle(
        CadastrarTaxaCommand command,
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
            Taxa taxa = new(
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.Nome,
                command.Valor,
                command.TipoCobranca
            );

            await repositorioTaxa.CadastrarAsync(taxa);

            await appDbContext.SaveChangesAsync(cancellationToken);

            CadastrarTaxaResult result = new(taxa.Id);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante o cadastro de {@Command}.", command);

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
