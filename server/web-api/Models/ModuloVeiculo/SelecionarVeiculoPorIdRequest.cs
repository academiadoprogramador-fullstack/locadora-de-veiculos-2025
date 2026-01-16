using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloVeiculo;

public record SelecionarVeiculoPorIdResponse(
    Guid Id,
    SelecionarGruposVeiculosDto GrupoVeiculos,
    string Modelo,
    string Marca,
    int Ano,
    string? Imagem,
    decimal CapacidadeTanque,
    TipoCombustivel TipoCombustivel
);
