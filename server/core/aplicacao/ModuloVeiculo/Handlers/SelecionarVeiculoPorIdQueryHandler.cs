using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculo;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Handlers;

public class SelecionarVeiculoPorIdQueryHandler(RepositorioVeiculoEmOrm repositorioVeiculo)
    : IRequestHandler<SelecionarVeiculoPorIdQuery, Result<SelecionarVeiculoPorIdResult>>
{
    public async Task<Result<SelecionarVeiculoPorIdResult>> Handle(
        SelecionarVeiculoPorIdQuery query,
        CancellationToken cancellationToken
    )
    {
        Veiculo? registroEncontrado = await repositorioVeiculo.SelecionarPorIdAsync(query.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

        SelecionarVeiculoPorIdResult response = new(
            query.Id,
            new SelecionarGruposVeiculosDto(
                registroEncontrado.GrupoVeiculosId,
                registroEncontrado.GrupoVeiculos?.Nome ?? string.Empty
            ),
            registroEncontrado.Modelo,
            registroEncontrado.Marca,
            registroEncontrado.Ano,
            registroEncontrado.Imagem,
            registroEncontrado.CapacidadeTanque,
            registroEncontrado.TipoCombustivel
        );

        return Result.Ok(response);
    }
}
