using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Commands;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using LocadoraDeVeiculos.WebApi.Models.ModuloCombustivel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize(Roles = "Empresa,Funcionario")]
[Route("api/configuracoes-combustiveis")]
public sealed class ConfiguracoesCombustiveisController(IMediator mediator) : MainController
{
    [HttpPost]
    public async Task<ActionResult<CadastrarConfiguracaoCombustiveisResponse>> Cadastrar(
        [FromBody] CadastrarConfiguracaoCombustiveisRequest request,
        CancellationToken cancellationToken
    )
    {
        CadastrarConfiguracaoCombustiveisCommand command = new(
            request.ValorAlcool,
            request.ValorDiesel,
            request.ValorEletricidade,
            request.ValorGas,
            request.ValorGasolina
        );

        Result<CadastrarConfiguracaoCombustiveisResult> result =
            await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new CadastrarConfiguracaoCombustiveisResponse(valor.Id);

            return CreatedAtAction(nameof(SelecionarUltima), new { }, response);
        });
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarConfiguracoesCombustiveisResponse>> SelecionarTodas(
        CancellationToken cancellationToken
    )
    {
        SelecionarConfiguracoesCombustiveisQuery query = new();

        Result<SelecionarConfiguracoesCombustiveisResult> result =
            await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new SelecionarConfiguracoesCombustiveisResponse(valor.Registros);

            return Ok(response);
        });
    }

    [HttpGet("ultima")]
    public async Task<ActionResult<SelecionarUltimaConfiguracaoCombustiveisResponse>> SelecionarUltima(
        CancellationToken cancellationToken
    )
    {
        SelecionarUltimaConfiguracaoCombustiveisQuery query = new();

        Result<SelecionarUltimaConfiguracaoCombustiveisResult> result =
            await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new SelecionarUltimaConfiguracaoCombustiveisResponse(
                valor.Id,
                valor.CriadaEm,
                valor.ValorAlcool,
                valor.ValorDiesel,
                valor.ValorEletricidade,
                valor.ValorGas,
                valor.ValorGasolina
            );

            return Ok(response);
        });
    }
}