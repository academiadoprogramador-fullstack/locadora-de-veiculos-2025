using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;

namespace LocadoraDeVeiculos.Dominio.ModuloCombustivel;

public class ConfiguracaoCombustiveis
{
    public Guid Id { get; set; }
    public DateTimeOffset CriadaEm { get; set; }

    public Guid EmpresaId { get; set; }
    public Usuario? Empresa { get; set; }

    public decimal ValorAlcool { get; set; }
    public decimal ValorDiesel { get; set; }
    public decimal ValorEletricidade { get; set; }
    public decimal ValorGas { get; set; }
    public decimal ValorGasolina { get; set; }

    public ConfiguracaoCombustiveis(
        Guid empresaId,
        decimal valorAlcool,
        decimal valorDiesel,
        decimal valorEletricidade,
        decimal valorGas,
        decimal valorGasolina
    )
    {
        Id = Guid.NewGuid();
        CriadaEm = DateTimeOffset.UtcNow;
        EmpresaId = empresaId;
        ValorAlcool = valorAlcool;
        ValorDiesel = valorDiesel;
        ValorEletricidade = valorEletricidade;
        ValorGas = valorGas;
        ValorGasolina = valorGasolina;
    }

    public decimal ObterValorCombustivel(TipoCombustivel tipoCombustivel)
    {
        return tipoCombustivel switch
        {
            TipoCombustivel.Alcool => ValorAlcool,
            TipoCombustivel.Diesel => ValorDiesel,
            TipoCombustivel.Gas => ValorGas,
            TipoCombustivel.Eletricidade => ValorEletricidade,
            _ => ValorGasolina
        };
    }
}
