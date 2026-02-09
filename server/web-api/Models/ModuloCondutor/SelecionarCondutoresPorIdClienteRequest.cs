using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloCliente;

public sealed record SelecionarCondutoresPorClienteIdResponse(
    IReadOnlyList<SelecionarCondutoresDto> Registros
);