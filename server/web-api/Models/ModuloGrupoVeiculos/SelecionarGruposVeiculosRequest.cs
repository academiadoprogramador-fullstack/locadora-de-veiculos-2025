using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloGrupoVeiculos;

public record SelecionarGruposVeiculosRequest();

public record SelecionarGruposVeiculosResponse(IReadOnlyList<SelecionarGruposVeiculosDto> Registros);
