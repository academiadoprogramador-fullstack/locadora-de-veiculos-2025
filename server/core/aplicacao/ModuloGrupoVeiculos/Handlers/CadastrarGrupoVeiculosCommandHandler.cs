using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Handlers;

public class CadastrarGrupoVeiculosCommandHandler(
    AppDbContext appDbContext,
    RepositorioGrupoVeiculosEmOrm repositorioGrupoVeiculos,
    ITenantProvider tenantProvider,
    IValidator<CadastrarGrupoVeiculosCommand> validator,
    ILogger<CadastrarGrupoVeiculosCommandHandler> logger
) 
    : IRequestHandler<CadastrarGrupoVeiculosCommand, Result<CadastrarGrupoVeiculosResult>>
{
    public async Task<Result<CadastrarGrupoVeiculosResult>> Handle(
        CadastrarGrupoVeiculosCommand command, CancellationToken cancellationToken)
    {
        ValidationResult resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);
        
        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        List<GrupoVeiculos> registros = await repositorioGrupoVeiculos.SelecionarTodosAsync();

        if (registros.Any(x => x.Nome.Equals(command.Nome)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Um grupo de veículos com este nome já existe."));

		try
		{
            GrupoVeiculos grupoVeiculos = new(
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.Nome
            );

            await repositorioGrupoVeiculos.CadastrarAsync(grupoVeiculos);

            await appDbContext.SaveChangesAsync(cancellationToken);

            CadastrarGrupoVeiculosResult result = new(grupoVeiculos.Id);

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
