using LocadoraDeVeiculos.Dominio.ModuloVeiculo;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloVeiculo;

public record CadastrarVeiculoRequest(
    Guid GrupoVeiculosId,
    string Modelo,
    string Marca,
    int Ano,
    IFormFile? Imagem,
    decimal CapacidadeTanque,
    TipoCombustivel TipoCombustivel
);

public record CadastrarVeiculoResponse(Guid Id);
