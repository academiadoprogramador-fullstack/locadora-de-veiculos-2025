using LocadoraDeVeiculos.Dominio.ModuloVeiculo;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloVeiculo;

public record EditarVeiculoRequest(
    Guid GrupoVeiculosId,
    string Modelo,
    string Marca,
    int Ano,
    IFormFile? Imagem,
    decimal CapacidadeTanque,
    TipoCombustivel TipoCombustivel
);

public record EditarVeiculoResponse(
    Guid GrupoVeiculosId,
    string Modelo,
    string Marca,
    int Ano,
    string? Imagem,
    decimal CapacidadeTanque,
    TipoCombustivel TipoCombustivel
);
