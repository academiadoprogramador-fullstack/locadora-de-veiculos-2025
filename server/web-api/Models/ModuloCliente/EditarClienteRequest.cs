using LocadoraDeVeiculos.Dominio.ModuloCliente;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloCliente;

public sealed record EditarClienteRequest(
    string Nome,
    string Email,
    string Telefone,
    TipoCliente Tipo,
    string NumeroDocumento,
    string Cidade,
    string Estado,
    string Bairro,
    string Rua,
    string Numero
);

public sealed record EditarClienteResponse(
    string Nome,
    string Email,
    string Telefone,
    TipoCliente Tipo,
    string NumeroDocumento,
    string Cidade,
    string Estado,
    string Bairro,
    string Rua,
    string Numero
);
