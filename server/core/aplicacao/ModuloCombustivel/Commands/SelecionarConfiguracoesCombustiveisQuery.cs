using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Commands;

public record SelecionarConfiguracoesCombustiveisQuery 
    : IRequest<Result<SelecionarConfiguracoesCombustiveisResult>>;

public record SelecionarConfiguracoesCombustiveisResult(
    IReadOnlyList<SelecionarConfiguracoesCombustiveisDto> Registros
);

public record SelecionarConfiguracoesCombustiveisDto(
    Guid Id,
    DateTimeOffset CriadaEm,
    decimal ValorAlcool,
    decimal ValorDiesel,
    decimal ValorEletricidade,
    decimal ValorGas,
    decimal ValorGasolina
);
