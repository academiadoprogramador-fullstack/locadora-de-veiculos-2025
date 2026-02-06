using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;

public record SelecionarClientesQuery() : IRequest<Result<SelecionarClientesResult>>;

public record SelecionarClientesResult(
    IReadOnlyList<SelecionarClientesDto> Registros
);

public record SelecionarClientesDto(
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