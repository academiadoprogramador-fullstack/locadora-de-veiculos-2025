using FluentResults;
using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxa;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Handlers;

public sealed class SimularAberturaAluguelCommandHandler(
    AppDbContext appDbContext,
    RepositorioAluguelEmOrm repositorioAluguel,
    RepositorioTaxaEmOrm repositorioTaxa,
    RepositorioVeiculoEmOrm repositorioVeiculo,
    RepositorioPlanoCobrancaEmOrm repositorioPlanoCobranca,
    ITenantProvider tenantProvider,
    IValidator<SimularAberturaAluguelCommand> validator,
    ILogger<SimularAberturaAluguelCommandHandler> logger
) : IRequestHandler<SimularAberturaAluguelCommand, Result<SimularAberturaAluguelResult>>
{
    public async Task<Result<SimularAberturaAluguelResult>> Handle(
        SimularAberturaAluguelCommand command,
        CancellationToken cancellationToken
    )
    {
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        try
        {
            var empresaId = tenantProvider.EmpresaId
                ?? throw new InvalidOperationException("Empresa não identificada.");

            var veiculo = await repositorioVeiculo.SelecionarPorIdAsync(command.VeiculoId);

            if (veiculo is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.VeiculoId));

            var planoCobranca = await repositorioPlanoCobranca
                .SelecionarPlanoDeCobrancaPorGrupoVeiculosIdAsync(veiculo.GrupoVeiculosId);

            if (planoCobranca is null)
            {
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                    "Não foi possível encontrar o plano de cobranças relacionado ao grupo de veículos selecionado."
                ));
            }

            var taxas = command.TaxasSelecionadasIds?.Count > 0
                ? await repositorioTaxa.SelecionarMuitosPorIdsAsync(command.TaxasSelecionadasIds)
                : [];

            var aluguel = new Aluguel(
                empresaId,
                command.CondutorId,
                command.VeiculoId,
                command.ConfiguracaoCombustiveisId,
                command.TipoPlano,
                command.InicioEmUtc,
                command.DevolucaoPrevistaEmUtc,
                taxas
            );

            await repositorioAluguel.CadastrarAsync(aluguel);

            await appDbContext.SaveChangesAsync(cancellationToken);

            var detalhamento = aluguel.CalcularValorParcialDetalhado(planoCobranca);

            var result = new SimularAberturaAluguelResult(
                aluguel.Id,
                detalhamento.QuantidadeDias,
                detalhamento.ValorPlano,
                detalhamento.ValorTaxas,
                detalhamento.ValorParcial
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante o cadastro de {@Command}.", command);

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}