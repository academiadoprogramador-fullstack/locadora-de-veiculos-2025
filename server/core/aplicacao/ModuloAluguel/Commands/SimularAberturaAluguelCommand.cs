using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

public record SimularAberturaAluguelCommand(
    Guid CondutorId,
    Guid VeiculoId,
    Guid ConfiguracaoCombustiveisId,
    TipoPlanoCobranca TipoPlano,
    DateTimeOffset InicioEmUtc,
    DateTimeOffset DevolucaoPrevistaEmUtc,
    IReadOnlyList<Guid> TaxasSelecionadasIds
) : IRequest<Result<SimularAberturaAluguelResult>>;

public record SimularAberturaAluguelResult(
    Guid Id,
    int QuantidadeDias,
    decimal ValorPlano,
    decimal ValorTaxas,
    decimal ValorParcial
);
