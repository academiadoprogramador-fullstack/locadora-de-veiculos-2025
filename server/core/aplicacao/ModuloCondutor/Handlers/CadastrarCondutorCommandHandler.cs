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

public sealed class CadastrarCondutorCommandHandler(
    AppDbContext appDbContext,
    RepositorioCondutorEmOrm repositorioCondutor,
    ITenantProvider tenantProvider,
    IValidator<CadastrarCondutorCommand> validator,
    ILogger<CadastrarCondutorCommandHandler> logger
) : IRequestHandler<CadastrarCondutorCommand, Result<CadastrarCondutorResult>>
{
    public async Task<Result<CadastrarCondutorResult>> Handle(
        CadastrarCondutorCommand command,
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
            Condutor condutor = new(
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

            await repositorioCondutor.CadastrarAsync(condutor);

            await appDbContext.SaveChangesAsync(cancellationToken);

            CadastrarCondutorResult result = new(condutor.Id);

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
