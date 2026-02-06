using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Handlers;

public sealed class SelecionarClientePorIdQueryHandler(
    RepositorioClienteEmOrm repositorioCliente
) : IRequestHandler<SelecionarClientePorIdQuery, Result<SelecionarClientePorIdResult>>
{
    public async Task<Result<SelecionarClientePorIdResult>> Handle(
        SelecionarClientePorIdQuery query,
        CancellationToken cancellationToken
    )
    {
        Cliente? registroEncontrado = await repositorioCliente.SelecionarPorIdAsync(query.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

        SelecionarClientePorIdResult response = new(
            registroEncontrado.Id,
            registroEncontrado.Nome,
            registroEncontrado.Email,
            registroEncontrado.Telefone,
            registroEncontrado.Tipo,
            registroEncontrado.NumeroDocumento,
            registroEncontrado.Cidade,
            registroEncontrado.Estado,
            registroEncontrado.Bairro,
            registroEncontrado.Rua,
            registroEncontrado.Numero
        );

        return Result.Ok(response);
    }
}
