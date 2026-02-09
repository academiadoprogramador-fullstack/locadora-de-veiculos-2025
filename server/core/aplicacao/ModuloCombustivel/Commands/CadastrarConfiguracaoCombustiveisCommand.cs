using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Commands;

public record CadastrarConfiguracaoCombustiveisCommand(
    decimal ValorAlcool,
    decimal ValorDiesel,
    decimal ValorEletricidade,
    decimal ValorGas,
    decimal ValorGasolina
) : IRequest<Result<CadastrarConfiguracaoCombustiveisResult>>;

public record CadastrarConfiguracaoCombustiveisResult(Guid Id);