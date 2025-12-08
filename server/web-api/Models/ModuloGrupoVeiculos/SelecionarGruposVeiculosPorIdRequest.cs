namespace LocadoraDeVeiculos.WebApi.Models.ModuloGrupoVeiculos;

public record SelecionarGruposVeiculosPorIdRequest(Guid Id);

public record SelecionarGrupoVeiculosPorIdResponse(Guid Id, string Nome);