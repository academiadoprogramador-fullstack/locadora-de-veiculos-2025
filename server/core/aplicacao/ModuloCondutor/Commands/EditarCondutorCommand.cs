using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;

public record EditarCondutorCommand(
    Guid Id,
    Guid ClienteId,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Telefone,
    string Cpf,
    string Cnh,
    DateTimeOffset ValidadeCnh
) : IRequest<Result<EditarCondutorResult>>;

public record EditarCondutorResult(
    Guid ClienteId,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Telefone,
    string Cpf,
    string Cnh,
    DateTimeOffset ValidadeCnh
);




