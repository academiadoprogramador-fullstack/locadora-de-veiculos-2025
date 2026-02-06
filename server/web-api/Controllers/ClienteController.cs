using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using LocadoraDeVeiculos.WebApi.Models.ModuloCliente;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize(Roles = "Empresa,Funcionario")]
[Route("api/clientes")]
public class ClienteController(IMediator mediator) : MainController
{
    [HttpPost]
    public async Task<ActionResult<CadastrarClienteResponse>> Cadastrar(
    [FromBody] CadastrarClienteRequest request,
    CancellationToken cancellationToken
)
    {
        CadastrarClienteCommand command = new(
            request.Nome,
            request.Email,
            request.Telefone,
            request.Tipo,
            request.NumeroDocumento,
            request.Cidade,
            request.Estado,
            request.Bairro,
            request.Rua,
            request.Numero
        );

        Result<CadastrarClienteResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, valor =>
        {
            CadastrarClienteResponse response = new(valor.Id);

            return CreatedAtAction(nameof(SelecionarPorId), new { id = valor.Id }, response);
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EditarClienteResponse>> Editar(
        Guid id,
        [FromBody] EditarClienteRequest request,
        CancellationToken cancellationToken
    )
    {
        EditarClienteCommand command = new(
            id,
            request.Nome,
            request.Email,
            request.Telefone,
            request.Tipo,
            request.NumeroDocumento,
            request.Cidade,
            request.Estado,
            request.Bairro,
            request.Rua,
            request.Numero
        );

        Result<EditarClienteResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, valor =>
        {
            EditarClienteResponse response = new(
                valor.Nome,
                valor.Email,
                valor.Telefone,
                valor.Tipo,
                valor.NumeroDocumento,
                valor.Cidade,
                valor.Estado,
                valor.Bairro,
                valor.Rua,
                valor.Numero
            );

            return Ok(response);
        });
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Excluir(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        ExcluirClienteCommand command = new(id);

        Result<ExcluirClienteResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, _ => NoContent());
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarClientesResponse>> SelecionarTodos(
        CancellationToken cancellationToken
    )
    {
        SelecionarClientesQuery query = new();

        Result<SelecionarClientesResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, valor =>
        {
            SelecionarClientesResponse response = new(valor.Registros);

            return Ok(response);
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarClientePorIdResponse>> SelecionarPorId(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        SelecionarClientePorIdQuery query = new(id);

        Result<SelecionarClientePorIdResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, valor =>
        {
            SelecionarClientePorIdResponse response = new(
                valor.Id,
                valor.Nome,
                valor.Email,
                valor.Telefone,
                valor.Tipo,
                valor.NumeroDocumento,
                valor.Cidade,
                valor.Estado,
                valor.Bairro,
                valor.Rua,
                valor.Numero
            );

            return Ok(response);
        });
    }
}
