using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloVeiculo;

public record SelecionarVeiculosResponse(
    IReadOnlyList<SelecionarVeiculosDto> Registros
);
