using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands;

public record EditarFuncionarioCommand(
    Guid Id,
    string NomeCompleto,
    string Cpf,
    decimal Salario,
    DateTimeOffset AdmissaoEmUtc
) : IRequest<Result<EditarFuncionarioResult>>;

public record EditarFuncionarioResult(
    string NomeCompleto,
    string Cpf,
    decimal Salario,
    DateTimeOffset AdmissaoEmUtc
);
