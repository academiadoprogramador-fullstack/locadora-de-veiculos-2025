using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;

public record SelecionarClientePorIdQuery(Guid Id)
    : IRequest<Result<SelecionarClientePorIdResult>>;

public record SelecionarClientePorIdResult(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    TipoCliente Tipo,
    string NumeroDocumento,
    string Cidade,
    string Estado,
    string Bairro,
    string Rua,
    string Numero
);
