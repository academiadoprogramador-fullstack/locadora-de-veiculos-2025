using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands;

public record CadastrarFuncionarioCommand(
    string NomeCompleto,
    string Cpf,
    string Email,
    string Senha,
    string ConfirmarSenha,
    decimal Salario,
    DateTimeOffset AdmissaoEmUtc
) : IRequest<Result<CadastrarFuncionarioResult>>;

public record CadastrarFuncionarioResult(Guid Id);
