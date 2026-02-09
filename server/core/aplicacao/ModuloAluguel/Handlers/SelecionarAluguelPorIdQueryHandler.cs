using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Handlers;

public sealed class SelecionarAluguelPorIdQueryHandler(
    RepositorioAluguelEmOrm repositorioAluguel,
    RepositorioPlanoCobrancaEmOrm repositorioPlanoCobranca
) : IRequestHandler<SelecionarAluguelPorIdQuery, Result<SelecionarAluguelPorIdResult>>
{
    public async Task<Result<SelecionarAluguelPorIdResult>> Handle(
        SelecionarAluguelPorIdQuery query,
        CancellationToken cancellationToken
    )
    {
        Aluguel? aluguel = await repositorioAluguel.SelecionarPorIdAsync(query.Id);

        if (aluguel is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

        if (aluguel.Veiculo is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro("Veículo do aluguel não carregado."));

        var planoSelecionado = await repositorioPlanoCobranca
            .SelecionarPlanoDeCobrancaPorGrupoVeiculosIdAsync(aluguel.Veiculo.GrupoVeiculosId);

        if (planoSelecionado is null)
        {
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                "Não foi possível encontrar o plano de cobrança do grupo de veículos do aluguel."
            ));
        }

        var condutorDto = new SelecionarCondutorDto(
            aluguel.CondutorId,
            aluguel.Condutor?.Nome ?? string.Empty
        );

        var veiculoDto = new SelecionarVeiculoDto(
            aluguel.VeiculoId,
            aluguel.Veiculo.Modelo ?? string.Empty,
            aluguel.Veiculo.Marca ?? string.Empty,
            aluguel.Veiculo.Ano
        );

        var config = aluguel.ConfiguracaoCombustiveis;

        var configDto = new SelecionarConfiguracaoCombustiveisDto(
            aluguel.ConfiguracaoCombustiveisId,
            config?.CriadaEm ?? default,
            config?.ValorAlcool ?? 0,
            config?.ValorDiesel ?? 0,
            config?.ValorEletricidade ?? 0,
            config?.ValorGas ?? 0,
            config?.ValorGasolina ?? 0
        );

        var taxas = (aluguel.TaxasSelecionadas ?? [])
            .Select(tx => new SelecionarTaxaDto(tx.Id, tx.Nome, tx.Valor, tx.TipoCobranca))
            .ToImmutableList();

        var parcial = aluguel.CalcularValorParcialDetalhado(planoSelecionado);

        SelecionarAluguelPorIdResult result = new(
            aluguel.Id,
            condutorDto,
            veiculoDto,
            configDto,
            aluguel.TipoPlano,
            aluguel.Status,
            aluguel.InicioEmUtc,
            aluguel.DevolucaoPrevistaEmUtc,
            aluguel.Devolucao?.OcorrenciaEmUtc,
            taxas   
        );

        return Result.Ok(result);
    }
}