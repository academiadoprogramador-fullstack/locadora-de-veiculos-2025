namespace LocadoraDeVeiculos.WebApi.Models.ModuloCombustivel;

public sealed record CadastrarConfiguracaoCombustiveisRequest(
    decimal ValorAlcool,
    decimal ValorDiesel,
    decimal ValorEletricidade,
    decimal ValorGas,
    decimal ValorGasolina
);

public sealed record CadastrarConfiguracaoCombustiveisResponse(Guid Id);