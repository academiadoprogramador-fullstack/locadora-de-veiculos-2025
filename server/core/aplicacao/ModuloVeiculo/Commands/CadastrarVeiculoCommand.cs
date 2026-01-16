using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;

public record CadastrarVeiculoCommand(
    Guid GrupoVeiculosId,
    string Modelo,
    string Marca,
    int Ano,
    IFormFile? Imagem,
    decimal CapacidadeTanque,
    TipoCombustivel TipoCombustivel
) : IRequest<Result<CadastrarVeiculoResult>>;

public record CadastrarVeiculoResult(Guid Id);
