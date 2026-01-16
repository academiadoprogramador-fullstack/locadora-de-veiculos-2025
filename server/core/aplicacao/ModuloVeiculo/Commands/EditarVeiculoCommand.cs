using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;

public record EditarVeiculoCommand(
    Guid Id,
    Guid GrupoVeiculosId,
    string Modelo,
    string Marca,
    int Ano,
    IFormFile? Imagem,
    decimal CapacidadeTanque,
    TipoCombustivel TipoCombustivel
) : IRequest<Result<EditarVeiculoResult>>;

public record EditarVeiculoResult(
    Guid GrupoVeiculosId,
    string Modelo,
    string Marca,
    int Ano,
    string? Imagem,
    decimal CapacidadeTanque,
    TipoCombustivel TipoCombustivel
);
