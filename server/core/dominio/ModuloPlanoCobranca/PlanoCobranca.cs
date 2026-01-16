using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;

namespace LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;

public class PlanoCobranca : EntidadeBase<PlanoCobranca>
{
    public Guid GrupoVeiculosId { get; set; }
    public GrupoVeiculos? GrupoVeiculos { get; set; }

    public decimal PrecoDiarioPlanoDiario { get; set; }
    public decimal PrecoQuilometroPlanoDiario { get; set; }

    public decimal QuilometrosDisponiveisPlanoControlado { get; set; }
    public decimal PrecoDiarioPlanoControlado { get; set; }
    public decimal PrecoQuilometroExtrapoladoPlanoControlado { get; set; }

    public decimal PrecoDiarioPlanoLivre { get; set; }

    public PlanoCobranca(
        Guid empresaId,
        Guid grupoVeiculosId,
        decimal precoDiarioPlanoDiario,
        decimal precoQuilometroPlanoDiario,
        decimal quilometrosDisponiveisPlanoControlado,
        decimal precoDiarioPlanoControlado,
        decimal precoQuilometroExtrapoladoPlanoControlado,
        decimal precoDiarioPlanoLivre
    )
    {
        EmpresaId = empresaId;
        GrupoVeiculosId = grupoVeiculosId;

        PrecoDiarioPlanoDiario = precoDiarioPlanoDiario;
        PrecoQuilometroPlanoDiario = precoQuilometroPlanoDiario;

        QuilometrosDisponiveisPlanoControlado = quilometrosDisponiveisPlanoControlado;
        PrecoDiarioPlanoControlado = precoDiarioPlanoControlado;
        PrecoQuilometroExtrapoladoPlanoControlado = precoQuilometroExtrapoladoPlanoControlado;

        PrecoDiarioPlanoLivre = precoDiarioPlanoLivre;
    }

    public override void AtualizarRegistro(PlanoCobranca registroEditado)
    {
        PrecoDiarioPlanoDiario = registroEditado.PrecoDiarioPlanoDiario;
        PrecoQuilometroPlanoDiario = registroEditado.PrecoQuilometroPlanoDiario;

        QuilometrosDisponiveisPlanoControlado = registroEditado.QuilometrosDisponiveisPlanoControlado;
        PrecoDiarioPlanoControlado = registroEditado.PrecoDiarioPlanoControlado;
        PrecoQuilometroExtrapoladoPlanoControlado = registroEditado.PrecoQuilometroExtrapoladoPlanoControlado;

        PrecoDiarioPlanoLivre = registroEditado.PrecoDiarioPlanoLivre;
    }

    public decimal CalcularValor(
        int quantidadeDeDias,
        int quilometragemPercorrida,
        TipoPlanoCobranca tipoPlano
    )
    {
        decimal valor = 0.0m;

        switch (tipoPlano)
        {
            case TipoPlanoCobranca.Diario:
                decimal valorDiasPlanoDiario = quantidadeDeDias * PrecoDiarioPlanoDiario;

                decimal valorQuilometragemPercorridaPlanoDiario =
                    quilometragemPercorrida * PrecoQuilometroPlanoDiario;

                valor = valorDiasPlanoDiario + valorQuilometragemPercorridaPlanoDiario;
                break;

            case TipoPlanoCobranca.Controlado:
                decimal valorDiasPlanoControlado = quantidadeDeDias * PrecoDiarioPlanoControlado;

                decimal quilometrosExtrapolados =
                    quilometragemPercorrida - QuilometrosDisponiveisPlanoControlado;

                decimal valorQuilometragemPlanoControlado =
                    quilometrosExtrapolados * PrecoQuilometroExtrapoladoPlanoControlado;

                valor = valorDiasPlanoControlado;

                if (quilometrosExtrapolados > 0) valor += valorQuilometragemPlanoControlado;
                break;

            case TipoPlanoCobranca.Livre:
                valor = quantidadeDeDias * PrecoDiarioPlanoDiario;
                break;
        }

        return valor;
    }
}