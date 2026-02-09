namespace LocadoraDeVeiculos.Dominio.ModuloAluguel;

public record DetalhamentoValorParcialAluguel(
    int QuantidadeDias,
    decimal ValorPlano,
    decimal ValorTaxas,
    decimal ValorParcial
);
