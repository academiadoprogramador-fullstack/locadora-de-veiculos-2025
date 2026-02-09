using LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloTaxa;

public sealed record SelecionarTaxasResponse(
    IReadOnlyList<SelecionarTaxasDto> Registros
);
