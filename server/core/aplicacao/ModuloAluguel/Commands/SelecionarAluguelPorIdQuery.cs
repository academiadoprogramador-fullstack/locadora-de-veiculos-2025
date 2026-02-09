using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

public record SelecionarAluguelPorIdQuery(Guid Id)
    : IRequest<Result<SelecionarAluguelPorIdResult>>;

public record SelecionarAluguelPorIdResult(
    Guid Id,
    SelecionarCondutorDto Condutor,
    SelecionarVeiculoDto Veiculo,
    SelecionarConfiguracaoCombustiveisDto ConfiguracaoCombustiveis,
    TipoPlanoCobranca TipoPlano,
    StatusAluguel Status,
    DateTimeOffset InicioEmUtc,
    DateTimeOffset DevolucaoPrevistaEmUtc,
    DateTimeOffset? DevolucaoEmUtc,
    IReadOnlyList<SelecionarTaxaDto> TaxasSelecionadas
);