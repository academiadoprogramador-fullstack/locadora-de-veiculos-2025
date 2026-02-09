using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxa;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Handlers;

using System.Collections.Immutable;

public sealed class SimularConclusaoAluguelQueryHandler(
    RepositorioAluguelEmOrm repositorioAluguel,
    RepositorioPlanoCobrancaEmOrm repositorioPlanoCobranca,
    RepositorioTaxaEmOrm repositorioTaxa,
    ILogger<SimularConclusaoAluguelQueryHandler> logger
) : IRequestHandler<SimularConclusaoAluguelQuery, Result<SimularConclusaoAluguelResult>>
{
    public async Task<Result<SimularConclusaoAluguelResult>> Handle(
        SimularConclusaoAluguelQuery query,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Aluguel? aluguel = await repositorioAluguel.SelecionarPorIdAsync(query.AluguelId);

            if (aluguel is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.AluguelId));

            if (aluguel.Status != StatusAluguel.Aberto)
            {
                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(
                    "Só é possível simular a conclusão de um aluguel com status 'Aberto'."
                ));
            }

            if (aluguel.Veiculo is null)
                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("É necessário carregar o veículo do aluguel."));

            if (aluguel.ConfiguracaoCombustiveis is null)
                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("É necessário carregar a configuração de combustíveis do aluguel."));

            var plano = await repositorioPlanoCobranca
                .SelecionarPlanoDeCobrancaPorGrupoVeiculosIdAsync(aluguel.Veiculo.GrupoVeiculosId);

            if (plano is null)
            {
                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(
                    "Não foi possível encontrar o plano de cobrança relacionado ao grupo do veículo do aluguel."
                ));
            }

            // Taxas consideradas (atuais + adicionais)
            var taxasIdsAtuais = (aluguel.TaxasSelecionadas ?? []).Select(t => t.Id);
            var taxasIdsAdicionais = query.TaxasAdicionaisIds ?? [];

            var taxasIdsConsideradas = taxasIdsAtuais
                .Concat(taxasIdsAdicionais)
                .Distinct()
                .ToImmutableList();

            List<Taxa> taxasConsideradas = taxasIdsConsideradas.Count == 0
                ? []
                : await repositorioTaxa.SelecionarMuitosPorIdsAsync(taxasIdsConsideradas);

            var aluguelSimulacao = new Aluguel(
                aluguel.EmpresaId,
                aluguel.CondutorId,
                aluguel.VeiculoId,
                aluguel.ConfiguracaoCombustiveisId,
                aluguel.TipoPlano,
                aluguel.InicioEmUtc,
                aluguel.DevolucaoPrevistaEmUtc,
                taxasConsideradas
            )
            {
                Status = aluguel.Status,
                Veiculo = aluguel.Veiculo,
                ConfiguracaoCombustiveis = aluguel.ConfiguracaoCombustiveis
            };

            var detalhado = aluguelSimulacao.CalcularValorTotalDetalhado(
                plano,
                query.MarcadorCombustivel,
                query.QuilometragemPercorrida
            );

            var result = new SimularConclusaoAluguelResult(
                query.AluguelId,
                detalhado.QuantidadeDias,
                detalhado.ValorPlano,
                detalhado.ValorTaxas,
                detalhado.ValorParcial,
                detalhado.TotalAbastecimento,
                detalhado.ValorMulta,
                detalhado.ValorTotal
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao simular conclusão do aluguel {@Query}.", query);

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}