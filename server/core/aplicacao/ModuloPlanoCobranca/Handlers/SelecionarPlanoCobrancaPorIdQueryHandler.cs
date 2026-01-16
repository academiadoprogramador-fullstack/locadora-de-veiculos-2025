using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Handlers;

public class SelecionarPlanoCobrancaPorIdQueryHandler(RepositorioPlanoCobrancaEmOrm repositorioPlanoCobranca)
    : IRequestHandler<SelecionarPlanoCobrancaPorIdQuery, Result<SelecionarPlanoCobrancaPorIdResult>>
{
    public async Task<Result<SelecionarPlanoCobrancaPorIdResult>> Handle(
    SelecionarPlanoCobrancaPorIdQuery query, CancellationToken cancellationToken)
    {
        PlanoCobranca? registroEncontrado = await repositorioPlanoCobranca.SelecionarPorIdAsync(query.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

        SelecionarPlanoCobrancaPorIdResult response = new(
            query.Id,
            new SelecionarGruposVeiculosDto(
                registroEncontrado.GrupoVeiculosId,
                registroEncontrado.GrupoVeiculos?.Nome ?? string.Empty
            ),
            new PlanoDiarioDto(
                registroEncontrado.PrecoDiarioPlanoDiario,
                registroEncontrado.PrecoQuilometroPlanoDiario
            ),
            new PlanoControladoDto(
                registroEncontrado.QuilometrosDisponiveisPlanoControlado,
                registroEncontrado.PrecoDiarioPlanoControlado,
                registroEncontrado.PrecoQuilometroExtrapoladoPlanoControlado
            ),
            new PlanoLivreDto(registroEncontrado.PrecoDiarioPlanoLivre)
        );

        return Result.Ok(response);
    }
}
