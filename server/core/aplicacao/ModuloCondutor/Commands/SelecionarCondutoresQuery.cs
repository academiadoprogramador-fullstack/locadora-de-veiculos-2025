using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;

public record SelecionarCondutoresQuery() : IRequest<Result<SelecionarCondutoresResult>>;

public record SelecionarCondutoresResult(
    IReadOnlyList<SelecionarCondutoresDto> Registros
);

public record SelecionarCondutoresDto(
    Guid Id,
    Guid ClienteId,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Telefone,
    string Cpf,
    string Cnh,
    DateTimeOffset ValidadeCnh
);




