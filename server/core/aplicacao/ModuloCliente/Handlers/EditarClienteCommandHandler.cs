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

public sealed class EditarClienteCommandHandler(
    AppDbContext appDbContext,
    RepositorioClienteEmOrm repositorioCliente,
    ITenantProvider tenantProvider,
    IValidator<EditarClienteCommand> validator,
    ILogger<EditarClienteCommandHandler> logger
) : IRequestHandler<EditarClienteCommand, Result<EditarClienteResult>>
{
    public async Task<Result<EditarClienteResult>> Handle(
        EditarClienteCommand command,
        CancellationToken cancellationToken
    )
    {
        ValidationResult resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        Cliente? registroEncontrado = await repositorioCliente.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            Cliente clienteEditado = new(
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

            await repositorioCliente.EditarAsync(command.Id, clienteEditado);

            await appDbContext.SaveChangesAsync(cancellationToken);

            EditarClienteResult result = new(
                clienteEditado.Nome,
                clienteEditado.Email,
                clienteEditado.Telefone,
                clienteEditado.Tipo,
                clienteEditado.NumeroDocumento,
                clienteEditado.Cidade,
                clienteEditado.Estado,
                clienteEditado.Bairro,
                clienteEditado.Rua,
                clienteEditado.Numero
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
