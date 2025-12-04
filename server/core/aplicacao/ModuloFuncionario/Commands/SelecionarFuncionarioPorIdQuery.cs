using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands;

public record SelecionarFuncionarioPorIdQuery(Guid Id) : IRequest<Result<SelecionarFuncionarioPorIdResult>>;

public record SelecionarFuncionarioPorIdResult(
    Guid Id,
    string NomeCompleto,
    string Email,
    decimal Salario,
    DateTimeOffset AdmissaoEmUtc
);