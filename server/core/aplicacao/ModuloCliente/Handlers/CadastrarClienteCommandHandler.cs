using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Handlers;

public sealed class CadastrarClienteCommandHandler(
    AppDbContext appDbContext,
    RepositorioClienteEmOrm repositorioCliente,
    ITenantProvider tenantProvider,
    IValidator<CadastrarClienteCommand> validator,
    ILogger<CadastrarClienteCommandHandler> logger
) : IRequestHandler<CadastrarClienteCommand, Result<CadastrarClienteResult>>
{
    public async Task<Result<CadastrarClienteResult>> Handle(
        CadastrarClienteCommand command,
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
            Cliente cliente = new(
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.Nome,
                command.Email,
                command.Telefone,
                command.Tipo,
                command.NumeroDocumento,
                command.Cidade,
                command.Estado,
                command.Bairro,
                command.Rua,
                command.Numero
            );

            await repositorioCliente.CadastrarAsync(cliente);

            await appDbContext.SaveChangesAsync(cancellationToken);

            CadastrarClienteResult result = new(cliente.Id);

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
