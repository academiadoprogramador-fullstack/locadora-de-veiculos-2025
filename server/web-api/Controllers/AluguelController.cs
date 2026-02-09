using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;
using LocadoraDeVeiculos.WebApi.Compartilhado;
using LocadoraDeVeiculos.WebApi.Models.ModuloAluguel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraDeVeiculos.WebApi.Controllers;

[Authorize(Roles = "Empresa,Funcionario")]
[Route("api/alugueis")]
public sealed class AluguelController(IMediator mediator) : MainController
{
    [HttpPost("simular-abertura")]
    public async Task<ActionResult<SimularAberturaAluguelResponse>> SimularAbertura(
        SimularAberturaAluguelRequest request,
        CancellationToken cancellationToken
    )
    {
        SimularAberturaAluguelCommand command = new(
            request.CondutorId,
            request.VeiculoId,
            request.ConfiguracaoCombustiveisId,
            request.TipoPlano,
            request.InicioEmUtc,
            request.DevolucaoPrevistaEmUtc,
            request.TaxasSelecionadasIds
        );

        Result<SimularAberturaAluguelResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new SimularAberturaAluguelResponse(
                valor.Id,
                valor.QuantidadeDias,
                valor.ValorPlano,
                valor.ValorTaxas,
                valor.ValorParcial
            );

            return CreatedAtAction(nameof(SelecionarPorId), new { id = valor.Id }, response);
        });
    }

    [HttpPost("{id:guid}/abrir")]
    public async Task<ActionResult<AbrirAluguelResponse>> Abrir(
         Guid id,
         CancellationToken cancellationToken
    )
    {
        AbrirAluguelCommand command = new(id);

        Result<AbrirAluguelResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, _ =>
        {
            AbrirAluguelResponse response = new();

            return Ok(response);
        });
    }

    [HttpPost("{id:guid}/simular-conclusao")]
    public async Task<ActionResult<SimularConclusaoAluguelResponse>> SimularConclusao(
       Guid id,
       SimularConclusaoAluguelRequest request,
       CancellationToken cancellationToken
   )
    {
        var query = new SimularConclusaoAluguelQuery(
            id,
            request.MarcadorCombustivel,
            request.QuilometragemPercorrida,
            request.TaxasAdicionaisIds
        );

        Result<SimularConclusaoAluguelResult> result =
            await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new SimularConclusaoAluguelResponse(
                valor.AluguelId,
                valor.QuantidadeDias,
                valor.ValorPlano,
                valor.ValorTaxas,
                valor.ValorParcial,
                valor.TotalAbastecimento,
                valor.ValorMulta,
                valor.ValorTotal
            );

            return Ok(response);
        });
    }

    [HttpPost("{id:guid}/concluir")]
    public async Task<ActionResult<ConcluirAluguelResponse>> Concluir(
        Guid id,
        ConcluirAluguelRequest request,
        CancellationToken cancellationToken
    )
    {
        ConcluirAluguelCommand command = new(
            id,
            request.QuilometragemPercorrida,
            request.MarcadorCombustivel,
            request.TaxasAdicionaisIds
        );

        Result<ConcluirAluguelResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            ConcluirAluguelResponse response = new(
                valor.Id,
                valor.DevolucaoEmUtc
            );

            return Ok(response);
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SimularAberturaAluguelResult>> Editar(
        Guid id,
        EditarAluguelRequest request,
        CancellationToken cancellationToken
    )
    {
        EditarAluguelCommand command = new(
            id,
            request.CondutorId,
            request.VeiculoId,
            request.TipoPlano,
            request.InicioEmUtc,
            request.DevolucaoPrevistaEmUtc,
            request.TaxasSelecionadasIds
        );

        Result<SimularAberturaAluguelResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var response = new SimularAberturaAluguelResponse(
                valor.Id,
                valor.QuantidadeDias,
                valor.ValorPlano,
                valor.ValorTaxas,
                valor.ValorParcial
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
        ExcluirAluguelCommand command = new(id);

        Result<ExcluirAluguelResult> result = await mediator.Send(command, cancellationToken);

        return ProcessarResultado(result, _ => NoContent());
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarAlugueisResponse>> SelecionarTodos(
        CancellationToken cancellationToken
    )
    {
        SelecionarAlugueisQuery query = new();

        Result<SelecionarAlugueisResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var registros = valor.Registros
                .Select(r => new SelecionarAlugueisDto(
                    r.Id,
                    r.CondutorId,
                    r.CondutorNome,
                    r.VeiculoId,
                    r.VeiculoDescricao,
                    r.ConfiguracaoCombustiveisId,
                    r.TipoPlano,
                    r.Status,
                    r.InicioEmUtc,
                    r.DevolucaoPrevistaEmUtc,
                    r.DevolucaoEmUtc
                ))
                .ToList()
                .AsReadOnly();

            SelecionarAlugueisResponse response = new(registros);

            return Ok(response);
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarAluguelPorIdResponse>> SelecionarPorId(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        SelecionarAluguelPorIdQuery query = new(id);

        Result<SelecionarAluguelPorIdResult> result = await mediator.Send(query, cancellationToken);

        return ProcessarResultado(result, (valor) =>
        {
            var condutor = new SelecionarCondutorDto(valor.Condutor.Id, valor.Condutor.Nome);

            var veiculo = new SelecionarVeiculoDto(
                valor.Veiculo.Id,
                valor.Veiculo.Modelo,
                valor.Veiculo.Marca,
                valor.Veiculo.Ano
            );

            var configuracao = new SelecionarConfiguracaoCombustiveisDto(
                valor.ConfiguracaoCombustiveis.Id,
                valor.ConfiguracaoCombustiveis.CriadaEm,
                valor.ConfiguracaoCombustiveis.ValorAlcool,
                valor.ConfiguracaoCombustiveis.ValorDiesel,
                valor.ConfiguracaoCombustiveis.ValorEletricidade,
                valor.ConfiguracaoCombustiveis.ValorGas,
                valor.ConfiguracaoCombustiveis.ValorGasolina
            );

            var taxas = valor.TaxasSelecionadas
                .Select(t => new SelecionarTaxaDto(t.Id, t.Nome, t.Valor, t.TipoCobranca))
                .ToList()
                .AsReadOnly();

            SelecionarAluguelPorIdResponse response = new(
                valor.Id,
                condutor,
                veiculo,
                configuracao,
                valor.TipoPlano,
                valor.Status,
                valor.InicioEmUtc,
                valor.DevolucaoPrevistaEmUtc,
                valor.DevolucaoEmUtc,
                taxas
            );

            return Ok(response);
        });
    }
}