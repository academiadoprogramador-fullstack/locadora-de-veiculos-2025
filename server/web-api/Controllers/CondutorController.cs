using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using LocadoraDeVeiculos.WebApi.Models.ModuloCliente;
using LocadoraDeVeiculos.WebApi.Models.ModuloCondutor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize(Roles = "Empresa,Funcionario")]
[Route("api/condutores")]
public sealed class CondutorController(IMediator mediator) : MainController
{
    [HttpPost]
    public async Task<ActionResult<CadastrarCondutorResponse>> Cadastrar(
        [FromBody] CadastrarCondutorRequest request,
        CancellationToken cancellationToken
    )
    {
        CadastrarCondutorCommand command = new(
            request.ClienteId,
            request.ClienteCondutor,
            request.Nome,
            request.Email,
            request.Telefone,
            request.Cpf,
            request.Cnh,
            request.ValidadeCnh
        );

        Result<CadastrarCondutorResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, valor =>
        {
            CadastrarCondutorResponse response = new(valor.Id);

            return CreatedAtAction(nameof(SelecionarPorId), new { id = valor.Id }, response);
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EditarCondutorResponse>> Editar(
        Guid id,
        [FromBody] EditarCondutorRequest request,
        CancellationToken cancellationToken
    )
    {
        EditarCondutorCommand command = new(
            id,
            request.ClienteId,
            request.ClienteCondutor,
            request.Nome,
            request.Email,
            request.Telefone,
            request.Cpf,
            request.Cnh,
            request.ValidadeCnh
        );

        Result<EditarCondutorResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, valor =>
        {
            EditarCondutorResponse response = new(
                valor.ClienteId,
                valor.ClienteCondutor,
                valor.Nome,
                valor.Email,
                valor.Telefone,
                valor.Cpf,
                valor.Cnh,
                valor.ValidadeCnh
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
        ExcluirCondutorCommand command = new(id);

        Result<ExcluirCondutorResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, _ => NoContent());
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarCondutoresResponse>> SelecionarTodos(
        CancellationToken cancellationToken
    )
    {
        SelecionarCondutoresQuery query = new();

        Result<SelecionarCondutoresResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, valor =>
        {
            SelecionarCondutoresResponse response = new(valor.Registros);

            return Ok(response);
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarCondutorPorIdResponse>> SelecionarPorId(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        SelecionarCondutorPorIdQuery query = new(id);

        Result<SelecionarCondutorPorIdResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, valor =>
        {
            SelecionarCondutorPorIdResponse response = new(
                valor.Id,
                valor.Cliente,
                valor.ClienteCondutor,
                valor.Nome,
                valor.Email,
                valor.Telefone,
                valor.Cpf,
                valor.Cnh,
                valor.ValidadeCnh
            );

            return Ok(response);
        });
    }

    [HttpGet("cliente/{id:guid}")]
    public async Task<ActionResult<SelecionarCondutoresPorClienteIdResponse>> SelecionarCondutoresDoCliente(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        SelecionarCondutoresPorClienteIdQuery query = new(id);

        Result<SelecionarCondutoresPorClienteIdResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, valor =>
        {
            SelecionarCondutoresPorClienteIdResponse response = new(valor.Registros);

            return Ok(response);
        });
    }
}