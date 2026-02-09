using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloCondutor;

public sealed record SelecionarCondutorPorIdResponse(
    Guid Id,
    SelecionarClienteDto Cliente,
    bool ClienteCondutor,
    string Nome,
    string Email,
    string Telefone,
    string Cpf,
    string Cnh,
    DateTimeOffset ValidadeCnh
);
