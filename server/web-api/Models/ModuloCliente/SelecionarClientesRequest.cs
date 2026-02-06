using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloCliente;

public sealed record SelecionarClientesResponse(
    IReadOnlyList<SelecionarClientesDto> Registros
);