using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

public record EditarAluguelCommand(
    Guid Id,
    Guid CondutorId,
    Guid VeiculoId,
    TipoPlanoCobranca TipoPlano,
    DateTimeOffset InicioEmUtc,
    DateTimeOffset DevolucaoPrevistaEmUtc,
    IReadOnlyList<Guid> TaxasSelecionadasIds
) : IRequest<Result<SimularAberturaAluguelResult>>;
