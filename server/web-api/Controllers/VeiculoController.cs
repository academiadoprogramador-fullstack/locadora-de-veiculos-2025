using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using LocadoraDeVeiculos.WebApi.Models.ModuloVeiculo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize(Roles = "Empresa,Funcionario")]
[Route("api/veiculos")]
public class VeiculoController(IMediator mediator) : MainController
{
    [HttpPost]
    public async Task<ActionResult<CadastrarVeiculoResponse>> Cadastrar(
        [FromForm] CadastrarVeiculoRequest request,
        CancellationToken cancellationToken
    )
    {
        CadastrarVeiculoCommand command = new(
            request.GrupoVeiculosId,
            request.Modelo,
            request.Marca,
            request.Ano,
            request.Imagem,
            request.CapacidadeTanque,
            request.TipoCombustivel
        );

        Result<CadastrarVeiculoResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            CadastrarVeiculoResponse response = new(valor.Id);

            return CreatedAtAction(nameof(SelecionarPorId), new { id = valor.Id }, response);
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EditarVeiculoResponse>> Editar(
       Guid id,
       [FromForm] EditarVeiculoRequest request,
       CancellationToken cancellationToken
    )
    {
        EditarVeiculoCommand command = new(
            id,
            request.GrupoVeiculosId,
            request.Modelo,
            request.Marca,
            request.Ano,
            request.Imagem,
            request.CapacidadeTanque,
            request.TipoCombustivel
        );

        Result<EditarVeiculoResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            EditarVeiculoResponse response = new(
                valor.GrupoVeiculosId,
                valor.Modelo,
                valor.Marca,
                valor.Ano,
                valor.Imagem,
                valor.CapacidadeTanque,
                valor.TipoCombustivel
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
        ExcluirVeiculoCommand command = new(id);

        Result<ExcluirVeiculoResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) => NoContent());
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarVeiculosResponse>> SelecionarTodos(
        CancellationToken cancellationToken
    )
    {
        SelecionarVeiculosQuery query = new();

        Result<SelecionarVeiculosResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            SelecionarVeiculosResponse response = new(valor.Registros);

            return Ok(response);
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarVeiculoPorIdResponse>> SelecionarPorId(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        SelecionarVeiculoPorIdQuery query = new(id);

        Result<SelecionarVeiculoPorIdResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            SelecionarVeiculoPorIdResponse response = new(
                valor.Id,
                valor.GrupoVeiculos,
                valor.Modelo,
                valor.Marca,
                valor.Ano,
                valor.Imagem,
                valor.CapacidadeTanque,
                valor.TipoCombustivel
            );

            return Ok(response);
        });
    }
}
