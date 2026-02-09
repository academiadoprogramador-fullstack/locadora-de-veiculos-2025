using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Handlers;

public sealed class SelecionarAlugueisQueryHandler(
    RepositorioAluguelEmOrm repositorioAluguel
) : IRequestHandler<SelecionarAlugueisQuery, Result<SelecionarAlugueisResult>>
{
    public async Task<Result<SelecionarAlugueisResult>> Handle(
        SelecionarAlugueisQuery request,
        CancellationToken cancellationToken
    )
    {
        var registros = await repositorioAluguel.SelecionarTodosAsync();

        var dtos = registros
            .Select(a => new SelecionarAlugueisDto(
                a.Id,
                a.CondutorId,
                a.Condutor?.Nome ?? string.Empty,
                a.VeiculoId,
                $"{a.Veiculo?.Marca ?? string.Empty} {a.Veiculo?.Modelo ?? string.Empty}".Trim(),
                a.ConfiguracaoCombustiveisId,
                a.TipoPlano,
                a.Status,
                a.InicioEmUtc,
                a.DevolucaoPrevistaEmUtc,
                a.Devolucao?.OcorrenciaEmUtc
            ))
            .ToImmutableList();

        var response = new SelecionarAlugueisResult(dtos);

        return Result.Ok(response);
    }
}