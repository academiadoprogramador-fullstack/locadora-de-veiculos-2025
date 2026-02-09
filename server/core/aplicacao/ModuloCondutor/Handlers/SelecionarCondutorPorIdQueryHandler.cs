using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCondutor;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Handlers;

public sealed class SelecionarCondutorPorIdQueryHandler(
    RepositorioCondutorEmOrm repositorioCondutor
) : IRequestHandler<SelecionarCondutorPorIdQuery, Result<SelecionarCondutorPorIdResult>>
{
    public async Task<Result<SelecionarCondutorPorIdResult>> Handle(
        SelecionarCondutorPorIdQuery query,
        CancellationToken cancellationToken
    )
    {
        Condutor? registroEncontrado = await repositorioCondutor.SelecionarPorIdAsync(query.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

        SelecionarCondutorPorIdResult response = new(
            registroEncontrado.Id,
            new SelecionarClienteDto(
                registroEncontrado.ClienteId,
                registroEncontrado.Cliente?.Nome ?? string.Empty
            ),
            registroEncontrado.ClienteCondutor,
            registroEncontrado.Nome,
            registroEncontrado.Email,
            registroEncontrado.Telefone,
            registroEncontrado.Cpf,
            registroEncontrado.Cnh,
            registroEncontrado.ValidadeCnh
        );

        return Result.Ok(response);
    }
}
