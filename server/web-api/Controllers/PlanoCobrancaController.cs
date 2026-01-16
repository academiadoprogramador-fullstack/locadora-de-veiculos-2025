using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using LocadoraDeVeiculos.WebApi.Models.ModuloPlanoCobranca;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize(Roles = "Empresa,Funcionario")]
[Route("api/planos-cobranca")]
public class PlanoCobrancaController(IMediator mediator) : MainController
{
    [HttpPost]
    public async Task<ActionResult<CadastrarPlanoCobrancaResponse>> Cadastrar(
        CadastrarPlanoCobrancaRequest request,
        CancellationToken cancellationToken
     )
    {
        CadastrarPlanoCobrancaCommand command = new(
            request.GrupoVeiculosId,
            request.PrecosPlanoDiario,
            request.PrecosPlanoControlado,
            request.PrecosPlanoLivre
        );

        Result<CadastrarPlanoCobrancaResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            CadastrarPlanoCobrancaResponse response = new(valor.Id);

            return CreatedAtAction(nameof(SelecionarPorId), new { id = valor.Id }, response);
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CadastrarPlanoCobrancaResponse>> Editar(
       Guid id,
       EditarPlanoCobrancaRequest request,
       CancellationToken cancellationToken
    )
    {
        EditarPlanoCobrancaCommand command = new(
            id,
            request.PrecosPlanoDiario,
            request.PrecosPlanoControlado,
            request.PrecosPlanoLivre
        );

        Result<EditarPlanoCobrancaResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            EditarPlanoCobrancaResponse response = new(
                valor.PrecosPlanoDiario,
                valor.PrecosPlanoControlado,
                valor.PrecosPlanoLivre
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
        ExcluirPlanoCobrancaCommand command = new(id);

        Result<ExcluirPlanoCobrancaResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) => NoContent());
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarPlanosCobrancaResponse>> SelecionarTodos(
        CancellationToken cancellationToken
    )
    {
        SelecionarPlanosCobrancaQuery query = new();

        Result<SelecionarPlanosCobrancaResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            SelecionarPlanosCobrancaResponse response = new(valor.Registros);

            return Ok(response);
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarPlanoCobrancaPorIdResponse>> SelecionarPorId(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        SelecionarPlanoCobrancaPorIdQuery query = new(id);

        Result<SelecionarPlanoCobrancaPorIdResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            SelecionarPlanoCobrancaPorIdResponse response = new(
                valor.Id,
                valor.GrupoVeiculos,
                valor.PrecosPlanoDiario,
                valor.PrecosPlanoControlado,
                valor.PrecosPlanoLivre
            );

            return Ok(response);
        });
    }
}
