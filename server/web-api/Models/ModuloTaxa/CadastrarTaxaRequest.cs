using LocadoraDeVeiculos.Dominio.ModuloTaxa;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloTaxa;

public sealed record CadastrarTaxaRequest(
    string Nome,
    decimal Valor,
    TipoCobrancaTaxa TipoCobranca
);

public sealed record CadastrarTaxaResponse(Guid Id);