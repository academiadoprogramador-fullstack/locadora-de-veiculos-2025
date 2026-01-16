using FluentResults;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;

public record SelecionarVeiculoPorIdQuery(Guid Id)
    : IRequest<Result<SelecionarVeiculoPorIdResult>>;

public record SelecionarVeiculoPorIdResult(
    Guid Id,
    SelecionarGruposVeiculosDto GrupoVeiculos,
    string Modelo,
    string Marca,
    int Ano,
    string? Imagem,
    decimal CapacidadeTanque,
    TipoCombustivel TipoCombustivel
);
