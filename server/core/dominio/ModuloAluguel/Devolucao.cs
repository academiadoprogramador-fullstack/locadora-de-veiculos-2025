using LocadoraDeVeiculos.Dominio.ModuloVeiculo;

namespace LocadoraDeVeiculos.Dominio.ModuloAluguel;

public sealed class Devolucao
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset OcorrenciaEmUtc { get; set; } = DateTimeOffset.UtcNow;

    public Guid AluguelId { get; set; }
    public MarcadorCombustivel MarcadorCombustivel { get; set; }
    public int QuilometragemPercorrida { get; set; }

    public Devolucao(
        Guid aluguelId,
        MarcadorCombustivel marcadorCombustivel,
        int quilometragemPercorrida
    )
    {
        AluguelId = aluguelId;
        QuilometragemPercorrida = quilometragemPercorrida;
        MarcadorCombustivel = marcadorCombustivel;
    }
}
