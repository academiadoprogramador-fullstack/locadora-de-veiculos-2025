using LocadoraDeVeiculos.Dominio.ModuloTaxa;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloTaxa;

public sealed record SelecionarTaxaPorIdResponse(
    Guid Id,
    string Nome,
    decimal Valor,
    TipoCobrancaTaxa TipoCobranca
);