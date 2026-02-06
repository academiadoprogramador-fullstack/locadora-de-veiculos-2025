using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;

public record CadastrarClienteCommand(
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
) : IRequest<Result<CadastrarClienteResult>>;

public record CadastrarClienteResult(Guid Id);