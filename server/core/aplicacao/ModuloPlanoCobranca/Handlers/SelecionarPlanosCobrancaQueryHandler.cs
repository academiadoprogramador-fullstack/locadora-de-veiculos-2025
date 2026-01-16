using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using MediatR;
using System.Collections.Immutable;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Handlers;

public class SelecionarPlanosCobrancaQueryHandler(
    RepositorioPlanoCobrancaEmOrm repositorioPlanoCobranca
) : IRequestHandler<SelecionarPlanosCobrancaQuery, Result<SelecionarPlanosCobrancaResult>>
{
    public async Task<Result<SelecionarPlanosCobrancaResult>> Handle(
        SelecionarPlanosCobrancaQuery request, CancellationToken cancellationToken)
    {
        var registros = await repositorioPlanoCobranca.SelecionarTodosAsync();

        var dtos = registros
            .Select(r => new SelecionarPlanosCobrancaDto(
                 r.Id,
                new SelecionarGruposVeiculosDto(
                    r.GrupoVeiculosId,
                    r.GrupoVeiculos?.Nome ?? string.Empty
                ),
                new PlanoDiarioDto(
                    r.PrecoDiarioPlanoDiario,
                    r.PrecoQuilometroPlanoDiario
                ),
                new PlanoControladoDto(
                    r.QuilometrosDisponiveisPlanoControlado,
                    r.PrecoDiarioPlanoControlado,
                    r.PrecoQuilometroExtrapoladoPlanoControlado
                ),
                new PlanoLivreDto(r.PrecoDiarioPlanoLivre)
            ))
            .ToImmutableList();

        var response = new SelecionarPlanosCobrancaResult(dtos);

        return Result.Ok(response);
    }
}
