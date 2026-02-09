using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;

public record SelecionarCondutorPorIdQuery(Guid Id)
    : IRequest<Result<SelecionarCondutorPorIdResult>>;

public record SelecionarCondutorPorIdResult(
    Guid Id,
    SelecionarClienteDto Cliente,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Telefone,
    string Cpf,
    string Cnh,
    DateTimeOffset ValidadeCnh
);

public record SelecionarClienteDto(
    Guid Id,
    string Nome
);
