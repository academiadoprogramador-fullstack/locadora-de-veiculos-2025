using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxa;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Handlers;

public sealed class EditarAluguelCommandHandler(
    AppDbContext appDbContext,
    RepositorioAluguelEmOrm repositorioAluguel,
    RepositorioTaxaEmOrm repositorioTaxa,
    RepositorioPlanoCobrancaEmOrm repositorioPlanoCobranca,
    IValidator<EditarAluguelCommand> validator,
    ILogger<EditarAluguelCommandHandler> logger
) : IRequestHandler<EditarAluguelCommand, Result<SimularAberturaAluguelResult>>
{
    public async Task<Result<SimularAberturaAluguelResult>> Handle(
        EditarAluguelCommand command,
        CancellationToken cancellationToken
    )
    {
        ValidationResult resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        Aluguel? registroEncontrado = await repositorioAluguel.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        var planoCobranca = await repositorioPlanoCobranca
            .SelecionarPlanoDeCobrancaPorGrupoVeiculosIdAsync(registroEncontrado.Veiculo!.GrupoVeiculosId);

        if (planoCobranca is null)
        {
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(
                "Não foi possível encontrar o plano de cobranças relacionado ao grupo de veículos selecionado."
            ));
        }

        try
        {
            var taxas = command.TaxasSelecionadasIds?.Count > 0
                ? await repositorioTaxa.SelecionarMuitosPorIdsAsync(command.TaxasSelecionadasIds)
                : [];

            Aluguel aluguelEditado = new(
                registroEncontrado.EmpresaId,
                command.CondutorId,
                command.VeiculoId,
                registroEncontrado.ConfiguracaoCombustiveisId,
                command.TipoPlano,
                command.InicioEmUtc,
                command.DevolucaoPrevistaEmUtc,
                taxas
            );

            aluguelEditado.Status = registroEncontrado.Status;

            await repositorioAluguel.EditarAsync(command.Id, aluguelEditado);

            await appDbContext.SaveChangesAsync(cancellationToken);

            var detalhamento = registroEncontrado.CalcularValorParcialDetalhado(planoCobranca);

            var result = new SimularAberturaAluguelResult(
                registroEncontrado.Id,
                detalhamento.QuantidadeDias,
                detalhamento.ValorPlano,
                detalhamento.ValorTaxas,
                detalhamento.ValorParcial
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a edição de {@Command}.", command);

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
