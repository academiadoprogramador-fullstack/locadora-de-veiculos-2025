using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCondutor;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Handlers;

public sealed class EditarCondutorCommandHandler(
    AppDbContext appDbContext,
    RepositorioCondutorEmOrm repositorioCondutor,
    ITenantProvider tenantProvider,
    IValidator<EditarCondutorCommand> validator,
    ILogger<EditarCondutorCommandHandler> logger
) : IRequestHandler<EditarCondutorCommand, Result<EditarCondutorResult>>
{
    public async Task<Result<EditarCondutorResult>> Handle(
        EditarCondutorCommand command,
        CancellationToken cancellationToken
    )
    {
        ValidationResult resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        Condutor? registroEncontrado = await repositorioCondutor.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            Condutor condutorEditado = new(
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.ClienteId,
                command.ClienteCondutor,
                command.Nome,
                command.Email,
                command.Telefone,
                command.Cpf,
                command.Cnh,
                command.ValidadeCnh
            );

            await repositorioCondutor.EditarAsync(command.Id, condutorEditado);

            await appDbContext.SaveChangesAsync(cancellationToken);

            EditarCondutorResult result = new(
                condutorEditado.ClienteId,
                condutorEditado.ClienteCondutor,
                condutorEditado.Nome,
                condutorEditado.Email,
                condutorEditado.Telefone,
                condutorEditado.Cpf,
                condutorEditado.Cnh,
                condutorEditado.ValidadeCnh
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
