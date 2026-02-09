using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloCondutor;

public sealed record SelecionarCondutoresResponse(
    IReadOnlyList<SelecionarCondutoresDto> Registros
);
