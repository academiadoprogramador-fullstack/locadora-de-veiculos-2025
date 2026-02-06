using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;

public record EditarClienteCommand(
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
) : IRequest<Result<EditarClienteResult>>;

public record EditarClienteResult(
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