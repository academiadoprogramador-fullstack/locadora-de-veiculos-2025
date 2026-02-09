namespace LocadoraDeVeiculos.WebApi.Models.ModuloCondutor;

public sealed record EditarCondutorRequest(
    Guid ClienteId,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Telefone,
    string Cpf,
    string Cnh,
    DateTimeOffset ValidadeCnh
);

public sealed record EditarCondutorResponse(
    Guid ClienteId,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Telefone,
    string Cpf,
    string Cnh,
    DateTimeOffset ValidadeCnh
);
