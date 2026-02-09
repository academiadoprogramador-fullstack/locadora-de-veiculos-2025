using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;

public record CadastrarCondutorCommand(
    Guid ClienteId,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Telefone,
    string Cpf,
    string Cnh,
    DateTimeOffset ValidadeCnh
) : IRequest<Result<CadastrarCondutorResult>>;

public record CadastrarCondutorResult(Guid Id);
