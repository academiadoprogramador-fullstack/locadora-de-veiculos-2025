using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloFuncionario;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Handlers;

public class EditarFuncionarioCommandHandler(
    AppDbContext appDbContext,
    RepositorioFuncionarioEmOrm repositorioFuncionario,
    ITenantProvider tenantProvider,
    IValidator<EditarFuncionarioCommand> validator,
    ILogger<EditarFuncionarioCommandHandler> logger
) : IRequestHandler<EditarFuncionarioCommand, Result<EditarFuncionarioResult>>
{
    public async Task<Result<EditarFuncionarioResult>> Handle(
        EditarFuncionarioCommand command, CancellationToken cancellationToken)
    {
        Funcionario? registroEncontrado = await repositorioFuncionario.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        ValidationResult resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        List<Funcionario> registros = await repositorioFuncionario.SelecionarTodosAsync();

        if (registros.Any(x => !x.Id.Equals(command.Id) && x.Cpf.Equals(command.Cpf)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Um funcionário com este CPF já está cadastrado."));

        try
        {
            Funcionario funcionarioEditado = new(
                registroEncontrado.UsuarioId,
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.NomeCompleto,
                command.Cpf,
                registroEncontrado.Email,
                command.Salario,
                command.AdmissaoEmUtc
            );

            await repositorioFuncionario.EditarAsync(command.Id, funcionarioEditado);

            await appDbContext.SaveChangesAsync(cancellationToken);

            EditarFuncionarioResult result = new(
                command.NomeCompleto,
                command.Cpf,
                command.Salario,
                command.AdmissaoEmUtc
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
