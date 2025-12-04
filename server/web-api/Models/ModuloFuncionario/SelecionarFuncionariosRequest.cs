using LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloFuncionario;

public record SelecionarFuncionariosRequest();

public record SelecionarFuncionariosResponse(IReadOnlyList<SelecionarFuncionariosDto> Registros);
