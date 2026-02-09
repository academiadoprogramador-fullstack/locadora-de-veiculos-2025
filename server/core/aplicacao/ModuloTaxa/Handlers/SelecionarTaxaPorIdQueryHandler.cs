using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxa;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Handlers;

public sealed class SelecionarTaxaPorIdQueryHandler(
    RepositorioTaxaEmOrm repositorioTaxa
) : IRequestHandler<SelecionarTaxaPorIdQuery, Result<SelecionarTaxaPorIdResult>>
{
    public async Task<Result<SelecionarTaxaPorIdResult>> Handle(
        SelecionarTaxaPorIdQuery query,
        CancellationToken cancellationToken
    )
    {
        Taxa? registroEncontrado = await repositorioTaxa.SelecionarPorIdAsync(query.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

        SelecionarTaxaPorIdResult response = new(
            registroEncontrado.Id,
            registroEncontrado.Nome,
            registroEncontrado.Valor,
            registroEncontrado.TipoCobranca
        );

        return Result.Ok(response);
    }
}
