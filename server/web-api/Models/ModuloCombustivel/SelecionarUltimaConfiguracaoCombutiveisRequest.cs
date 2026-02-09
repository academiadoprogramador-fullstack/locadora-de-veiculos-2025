namespace LocadoraDeVeiculos.WebApi.Models.ModuloCombustivel;

public sealed record SelecionarUltimaConfiguracaoCombustiveisResponse(
    Guid Id,
    DateTimeOffset CriadaEm,
    decimal ValorAlcool,
    decimal ValorDiesel,
    decimal ValorEletricidade,
    decimal ValorGas,
    decimal ValorGasolina
);