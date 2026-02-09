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

public sealed class EditarTaxaCommandHandler(
    AppDbContext appDbContext,
    RepositorioTaxaEmOrm repositorioTaxa,
    ITenantProvider tenantProvider,
    IValidator<EditarTaxaCommand> validator,
    ILogger<EditarTaxaCommandHandler> logger
) : IRequestHandler<EditarTaxaCommand, Result<EditarTaxaResult>>
{
    public async Task<Result<EditarTaxaResult>> Handle(
        EditarTaxaCommand command,
        CancellationToken cancellationToken
    )
    {
        ValidationResult resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        Taxa? registroEncontrado = await repositorioTaxa.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            Taxa taxaEditada = new(
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.Nome,
                command.Valor,
                command.TipoCobranca
            );

            await repositorioTaxa.EditarAsync(command.Id, taxaEditada);

            await appDbContext.SaveChangesAsync(cancellationToken);

            EditarTaxaResult result = new(
                taxaEditada.Nome,
                taxaEditada.Valor,
                taxaEditada.TipoCobranca
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a edição de {@Command}.", command);

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}