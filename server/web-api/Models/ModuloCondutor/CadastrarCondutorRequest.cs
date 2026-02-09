namespace LocadoraDeVeiculos.WebApi.Models.ModuloCondutor;

public sealed record CadastrarCondutorRequest(
    Guid ClienteId,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Telefone,
    string Cpf,
    string Cnh,
    DateTimeOffset ValidadeCnh
);

public sealed record CadastrarCondutorResponse(Guid Id);
