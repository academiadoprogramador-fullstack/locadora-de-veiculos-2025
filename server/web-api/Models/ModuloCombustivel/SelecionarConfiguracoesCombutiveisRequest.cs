using LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloCombustivel;

public sealed record SelecionarConfiguracoesCombustiveisResponse(
    IReadOnlyList<SelecionarConfiguracoesCombustiveisDto> Registros
);