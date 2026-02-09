using LocadoraDeVeiculos.Dominio.ModuloTaxa;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloTaxa;

public sealed record EditarTaxaRequest(
    string Nome,
    decimal Valor,
    TipoCobrancaTaxa TipoCobranca
);

public sealed record EditarTaxaResponse(
    string Nome,
    decimal Valor,
    TipoCobrancaTaxa TipoCobranca
);
