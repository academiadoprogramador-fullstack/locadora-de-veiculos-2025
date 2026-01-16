using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;

namespace LocadoraDeVeiculos.Dominio.ModuloVeiculo;

public sealed class Veiculo : EntidadeBase<Veiculo>
{
    public Guid GrupoVeiculosId { get; set; }
    public GrupoVeiculos? GrupoVeiculos { get; set; }

    public string Modelo { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string? Imagem { get; set; }
    public decimal CapacidadeTanque { get; set; }
    public TipoCombustivel TipoCombustivel { get; set; }
    public bool Alugado { get; set; }

    public Veiculo(
        Guid empresaId,
        Guid grupoVeiculosId,
        string modelo,
        string marca,
        int ano,
        string? imagem,
        decimal capacidadeTanque,
        TipoCombustivel tipoCombustivel
    )
    {
        EmpresaId = empresaId;
        GrupoVeiculosId = grupoVeiculosId;
        Modelo = modelo;
        Marca = marca;
        Ano = ano;
        Imagem = imagem;
        CapacidadeTanque = capacidadeTanque;
        TipoCombustivel = tipoCombustivel;
    }

    public override void AtualizarRegistro(Veiculo registroEditado)
    {
        GrupoVeiculosId = registroEditado.GrupoVeiculosId;
        Modelo = registroEditado.Modelo;
        Marca = registroEditado.Marca;
        Imagem = registroEditado.Imagem;
        CapacidadeTanque = registroEditado.CapacidadeTanque;
        TipoCombustivel = registroEditado.TipoCombustivel;
    }

    public void Alugar()
    {
        Alugado = true;
    }

    public void Desocupar()
    {
        Alugado = false;
    }

    public decimal CalcularLitrosParaAbastecimento(MarcadorCombustivel marcadorCombustivel)
    {
        switch (marcadorCombustivel)
        {
            case MarcadorCombustivel.Vazio: return CapacidadeTanque;

            case MarcadorCombustivel.UmQuarto: return CapacidadeTanque - (CapacidadeTanque * (1m / 4m));

            case MarcadorCombustivel.MeioTanque: return CapacidadeTanque - (CapacidadeTanque * (1m / 2m));

            case MarcadorCombustivel.TresQuartos: return CapacidadeTanque - (CapacidadeTanque * (3m / 4m));

            default:
                return 0;
        }
    }
}
