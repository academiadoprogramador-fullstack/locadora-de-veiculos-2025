using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using LocadoraDeVeiculos.WebApi.Models.ModuloGrupoVeiculos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize(Roles = "Empresa,Funcionario")]
[Route("api/grupos-veiculos")]
public class GrupoVeiculosController(IMediator mediator) : MainController
{
    [HttpPost]
    public async Task<ActionResult<CadastrarGrupoVeiculosResponse>> Cadastrar(
        CadastrarGrupoVeiculosRequest request,
        CancellationToken cancellationToken
     )
    {
        CadastrarGrupoVeiculosCommand command = new(request.Nome);

        Result<CadastrarGrupoVeiculosResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            CadastrarGrupoVeiculosResponse response = new(valor.Id);

            return CreatedAtAction(nameof(SelecionarPorId), new { id = valor.Id }, response);
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CadastrarGrupoVeiculosResponse>> Editar(
       Guid id,
       EditarGrupoVeiculosRequest request,
       CancellationToken cancellationToken
    )
    {
        EditarGrupoVeiculosCommand command = new(id, request.Nome);

        Result<EditarGrupoVeiculosResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            EditarGrupoVeiculosResponse response = new(valor.Nome);

            return Ok(response);
        });
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Excluir(
       Guid id,
       CancellationToken cancellationToken
    )
    {
        ExcluirGrupoVeiculosCommand command = new(id);

        Result<ExcluirGrupoVeiculosResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (_) => NoContent());
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarGruposVeiculosResponse>> SelecionarTodos(
        CancellationToken cancellationToken
    )
    {
        SelecionarGruposVeiculosQuery query = new();

        Result<SelecionarGruposVeiculosResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            SelecionarGruposVeiculosResponse response = new(valor.Registros);

            return Ok(response);
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarGrupoVeiculosPorIdResponse>> SelecionarPorId(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        SelecionarGrupoVeiculosPorIdQuery query = new(id);

        Result<SelecionarGrupoVeiculosPorIdResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            SelecionarGrupoVeiculosPorIdResponse response = new(valor.Id, valor.Nome);

            return Ok(response);
        });
    }
}
