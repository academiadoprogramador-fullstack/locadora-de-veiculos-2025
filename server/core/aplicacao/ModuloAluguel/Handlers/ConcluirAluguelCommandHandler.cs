using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxa;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Handlers;

public sealed class ConcluirAluguelCommandHandler(
    AppDbContext appDbContext,
    RepositorioAluguelEmOrm repositorioAluguel,
    RepositorioTaxaEmOrm repositorioTaxa,
    ILogger<ConcluirAluguelCommandHandler> logger
) : IRequestHandler<ConcluirAluguelCommand, Result<ConcluirAluguelResult>>
{
    public async Task<Result<ConcluirAluguelResult>> Handle(
        ConcluirAluguelCommand command,
        CancellationToken cancellationToken
    )
    {
        Aluguel? registroEncontrado = await repositorioAluguel.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        if (registroEncontrado.Status == StatusAluguel.Concluido)
            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("O aluguel já foi concluído."));

        try
        {
            // 1) Merge de taxas adicionais
            var taxasAdicionaisIds = command.TaxasAdicionaisIds?
                .Where(id => id != Guid.Empty)
                .ToList() ?? [];

            if (taxasAdicionaisIds.Count > 0)
            {
                var taxasAdicionais = await repositorioTaxa.SelecionarMuitosPorIdsAsync(taxasAdicionaisIds);

                // Evita duplicar taxas
                var idsJaSelecionados = registroEncontrado.TaxasSelecionadas
                    .Select(t => t.Id)
                    .ToHashSet();

                foreach (var taxa in taxasAdicionais)
                {
                    if (!idsJaSelecionados.Contains(taxa.Id))
                        registroEncontrado.TaxasSelecionadas.Add(taxa);
                }
            }

            // 2) Conclui (libera veículo e marca finalizado)
            Devolucao devolucao = registroEncontrado.Concluir(command.QuilometragemPercorrida, command.MarcadorCombustivel);

            await appDbContext.AddAsync(devolucao, cancellationToken);

            await appDbContext.SaveChangesAsync(cancellationToken);

            ConcluirAluguelResult result = new(
                registroEncontrado.Id,
                registroEncontrado.Devolucao!.OcorrenciaEmUtc
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a conclusão de {@Command}.", command);

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}