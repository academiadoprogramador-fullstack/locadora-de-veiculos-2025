using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;

namespace LocadoraDeVeiculos.WebApi.Models.ModuloAluguel;

public sealed record SelecionarAluguelPorIdResponse(
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