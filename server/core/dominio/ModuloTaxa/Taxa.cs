using LocadoraDeVeiculos.Dominio.Compartilhado;

namespace LocadoraDeVeiculos.Dominio.ModuloTaxa;

public class Taxa : EntidadeBase<Taxa>
{
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public TipoCobrancaTaxa TipoCobranca { get; set; }

    public Taxa(Guid empresaId, string nome, decimal valor, TipoCobrancaTaxa tipoCobranca)
    {
        EmpresaId = empresaId;
        Nome = nome;
        Valor = valor;
        TipoCobranca = tipoCobranca;
    }

    public override void AtualizarRegistro(Taxa registroEditado)
    {
        Nome = registroEditado.Nome;
        Valor = registroEditado.Valor;
        TipoCobranca = registroEditado.TipoCobranca;
    }

    public decimal CalcularValor(int quantidadeDeDias)
    {
        if (TipoCobranca == TipoCobrancaTaxa.Diaria)
            return Valor * (decimal)quantidadeDeDias;

        return Valor;
    }
}
