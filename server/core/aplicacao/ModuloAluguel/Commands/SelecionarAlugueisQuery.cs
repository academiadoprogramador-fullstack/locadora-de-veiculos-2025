using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

public record SelecionarAlugueisQuery() : IRequest<Result<SelecionarAlugueisResult>>;

public record SelecionarAlugueisResult(
    IReadOnlyList<SelecionarAlugueisDto> Registros
);

public record SelecionarAlugueisDto(
    Guid Id,
    Guid CondutorId,
    string CondutorNome,
    Guid VeiculoId,
    string VeiculoDescricao,
    Guid ConfiguracaoCombustiveisId,
    TipoPlanoCobranca TipoPlano,
    StatusAluguel Status,
    DateTimeOffset InicioEmUtc,
    DateTimeOffset DevolucaoPrevistaEmUtc,
    DateTimeOffset? DevolucaoEmUtc
);

public record SelecionarCondutorDto(
    Guid Id,
    string Nome
);

public record SelecionarVeiculoDto(
    Guid Id,
    string Modelo,
    string Marca,
    int Ano
);

public record SelecionarConfiguracaoCombustiveisDto(
    Guid Id,
    DateTimeOffset CriadaEm,
    decimal ValorAlcool,
    decimal ValorDiesel,
    decimal ValorEletricidade,
    decimal ValorGas,
    decimal ValorGasolina
);

public record SelecionarTaxaDto(
    Guid Id,
    string Nome,
    decimal Valor,
    TipoCobrancaTaxa TipoCobranca
);
