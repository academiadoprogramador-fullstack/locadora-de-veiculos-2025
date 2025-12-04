using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands;

public record SelecionarFuncionariosQuery(): IRequest<Result<SelecionarFuncionariosResult>>;

public record SelecionarFuncionariosResult(IReadOnlyList<SelecionarFuncionariosDto> Registros);

public record SelecionarFuncionariosDto(
    Guid Id,
    string NomeCompleto,
    string Email,
    decimal Salario,
    DateTimeOffset AdmissaoEmUtc
);