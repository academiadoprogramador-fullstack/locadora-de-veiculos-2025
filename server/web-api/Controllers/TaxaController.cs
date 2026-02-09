using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using LocadoraDeVeiculos.WebApi.Models.ModuloTaxa;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize(Roles = "Empresa,Funcionario")]
[Route("api/taxas")]
public sealed class TaxaController(IMediator mediator) : MainController
{
    [HttpPost]
    public async Task<ActionResult<CadastrarTaxaResponse>> Cadastrar(
        [FromBody] CadastrarTaxaRequest request,
        CancellationToken cancellationToken
    )
    {
        CadastrarTaxaCommand command = new(
            request.Nome,
            request.Valor,
            request.TipoCobranca
        );

        Result<CadastrarTaxaResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            CadastrarTaxaResponse response = new(valor.Id);

            return CreatedAtAction(nameof(SelecionarPorId), new { id = valor.Id }, response);
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EditarTaxaResponse>> Editar(
        Guid id,
        [FromBody] EditarTaxaRequest request,
        CancellationToken cancellationToken
    )
    {
        EditarTaxaCommand command = new(
            id,
            request.Nome,
            request.Valor,
            request.TipoCobranca
        );

        Result<EditarTaxaResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            EditarTaxaResponse response = new(
                valor.Nome,
                valor.Valor,
                valor.TipoCobranca
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
        ExcluirTaxaCommand command = new(id);

        Result<ExcluirTaxaResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (_) => NoContent());
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarTaxasResponse>> SelecionarTodos(
        CancellationToken cancellationToken
    )
    {
        SelecionarTaxasQuery query = new();

        Result<SelecionarTaxasResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new SelecionarTaxasResponse(valor.Registros);

            return Ok(response);
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarTaxaPorIdResponse>> SelecionarPorId(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        SelecionarTaxaPorIdQuery query = new(id);

        Result<SelecionarTaxaPorIdResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            SelecionarTaxaPorIdResponse response = new(
                valor.Id,
                valor.Nome,
                valor.Valor,
                valor.TipoCobranca
            );

            return Ok(response);
        });
    }
}