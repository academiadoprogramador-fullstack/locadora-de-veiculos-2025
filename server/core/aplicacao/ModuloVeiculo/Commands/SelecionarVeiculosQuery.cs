using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;

public record SelecionarVeiculosQuery() : IRequest<Result<SelecionarVeiculosResult>>;

public record SelecionarVeiculosResult(
    IReadOnlyList<SelecionarVeiculosDto> Registros
);

public record SelecionarVeiculosDto(
    Guid Id,
    Guid GrupoVeiculosId,
    string Modelo,
    string Marca,
    int Ano,
    string? Imagem,
    decimal CapacidadeTanque,
    TipoCombustivel TipoCombustivel
);