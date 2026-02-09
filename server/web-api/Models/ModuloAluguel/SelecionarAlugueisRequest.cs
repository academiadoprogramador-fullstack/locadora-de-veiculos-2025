using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloAluguel;

public sealed record SelecionarAlugueisResponse(
    IReadOnlyList<SelecionarAlugueisDto> Registros
);