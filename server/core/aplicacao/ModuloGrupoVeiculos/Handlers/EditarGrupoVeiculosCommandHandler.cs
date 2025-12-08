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

public class EditarGrupoVeiculosCommandHandler(
    AppDbContext appDbContext,
    RepositorioGrupoVeiculosEmOrm repositorioGrupoVeiculos,
    ITenantProvider tenantProvider,
    IValidator<EditarGrupoVeiculosCommand> validator,
    ILogger<EditarGrupoVeiculosCommandHandler> logger
) : IRequestHandler<EditarGrupoVeiculosCommand, Result<EditarGrupoVeiculosResult>>
{
    public async Task<Result<EditarGrupoVeiculosResult>> Handle(
        EditarGrupoVeiculosCommand command, CancellationToken cancellationToken)
    {
        GrupoVeiculos? registroEncontrado = await repositorioGrupoVeiculos.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        ValidationResult resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        List<GrupoVeiculos> registros = await repositorioGrupoVeiculos.SelecionarTodosAsync();

        if (registros.Any(x => !x.Id.Equals(command.Id) && x.Nome.Equals(command.Nome)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Um grupo de veículos com este nome já existe."));

        try
        {
            GrupoVeiculos grupoVeiculosEditado = new(
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.Nome
            );

            await repositorioGrupoVeiculos.EditarAsync(command.Id, grupoVeiculosEditado);

            await appDbContext.SaveChangesAsync(cancellationToken);

            EditarGrupoVeiculosResult result = new(command.Nome);

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
