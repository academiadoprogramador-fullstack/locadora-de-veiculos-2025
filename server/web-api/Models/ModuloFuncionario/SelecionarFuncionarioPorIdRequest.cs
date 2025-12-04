namespace LocadoraDeVeiculos.WebApi.Models.ModuloFuncionario;

public record SelecionarFuncionarioPorIdResponse(
    Guid Id,
    string NomeCompleto,
    string Email,
    decimal Salario,
    DateTimeOffset AdmissaoEmUtc
);
