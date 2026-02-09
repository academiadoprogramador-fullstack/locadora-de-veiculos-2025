namespace LocadoraDeVeiculos.Dominio.ModuloAluguel;

public record DetalhamentoValorTotalAluguel(
    int QuantidadeDias,
    decimal ValorPlano,
    decimal ValorTaxas,
    decimal ValorParcial,
    decimal TotalAbastecimento,
    decimal ValorMulta,
    decimal ValorTotal
);