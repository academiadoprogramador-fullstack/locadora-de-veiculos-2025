using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Commands;

public record SelecionarUltimaConfiguracaoCombustiveisQuery : IRequest<Result<SelecionarUltimaConfiguracaoCombustiveisResult>>;

public record SelecionarUltimaConfiguracaoCombustiveisResult(
    Guid Id,
    DateTimeOffset CriadaEm,
    decimal ValorAlcool,
    decimal ValorDiesel,
    decimal ValorEletricidade,
    decimal ValorGas,
    decimal ValorGasolina
);
