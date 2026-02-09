using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCombustivel;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;

namespace LocadoraDeVeiculos.Dominio.ModuloAluguel;

public sealed class Aluguel : EntidadeBase<Aluguel>
{
    public Guid CondutorId { get; set; }
    public Condutor? Condutor { get; set; }

    public Guid VeiculoId { get; set; }
    public Veiculo? Veiculo { get; set; }

    public Guid ConfiguracaoCombustiveisId { get; set; }
    public ConfiguracaoCombustiveis? ConfiguracaoCombustiveis { get; set; }

    public List<Taxa> TaxasSelecionadas { get; set; } = [];

    public Devolucao? Devolucao { get; set; }

    public TipoPlanoCobranca TipoPlano { get; set; }
    public StatusAluguel Status { get; set; } = StatusAluguel.Simulacao;

    public DateTimeOffset InicioEmUtc { get; set; }
    public DateTimeOffset DevolucaoPrevistaEmUtc { get; set; }

    public Aluguel(
        Guid empresaId,
        Guid condutorId,
        Guid veiculoId,
        Guid configuracaoCombustiveisId,
        TipoPlanoCobranca tipoPlano,
        DateTimeOffset inicioEmUtc,
        DateTimeOffset devolucaoPrevistaEmUtc
    )
    {
        EmpresaId = empresaId;
        CondutorId = condutorId;
        VeiculoId = veiculoId;
        TipoPlano = tipoPlano;
        ConfiguracaoCombustiveisId = configuracaoCombustiveisId;
        InicioEmUtc = inicioEmUtc;
        DevolucaoPrevistaEmUtc = devolucaoPrevistaEmUtc;
    }

    public Aluguel(
        Guid empresaId,
        Guid condutorId,
        Guid veiculoId,
        Guid configuracaoCombustiveisId,
        TipoPlanoCobranca tipoPlano,
        DateTimeOffset inicioEmUtc,
        DateTimeOffset devolucaoPrevistaEmUtc,
        List<Taxa> taxasSelecionadas
    )
    {
        EmpresaId = empresaId;
        CondutorId = condutorId;
        VeiculoId = veiculoId;
        TipoPlano = tipoPlano;
        ConfiguracaoCombustiveisId = configuracaoCombustiveisId;
        InicioEmUtc = inicioEmUtc;
        DevolucaoPrevistaEmUtc = devolucaoPrevistaEmUtc;
        TaxasSelecionadas = taxasSelecionadas;
    }

    public override void AtualizarRegistro(Aluguel registroEditado)
    {
        CondutorId = registroEditado.CondutorId;
        VeiculoId = registroEditado.VeiculoId;
        ConfiguracaoCombustiveisId = registroEditado.ConfiguracaoCombustiveisId;
        Status = registroEditado.Status;
        InicioEmUtc = registroEditado.InicioEmUtc;
        DevolucaoPrevistaEmUtc = registroEditado.DevolucaoPrevistaEmUtc;
        TaxasSelecionadas = registroEditado.TaxasSelecionadas;
        TipoPlano = registroEditado.TipoPlano;
    }

    public void Abrir()
    {
        if (Status != StatusAluguel.Simulacao)
            throw new ArgumentException("Não é possível abrir um aluguel que não esteja em simulação.");

        if (Veiculo is null)
            throw new ArgumentException("É necessário carregar o veículo do aluguel para concluir esta operação.");

        Veiculo.Alugar();

        Status = StatusAluguel.Aberto;
    }

    public Devolucao Concluir(int quilometragemPercorrida, MarcadorCombustivel marcadorCombustivel)
    {
        if (Veiculo is null)
            throw new ArgumentException("É necessário carregar o veículo do aluguel para concluir esta operação.");

        Veiculo.Desocupar();

        Status = StatusAluguel.Concluido;

        Devolucao = new Devolucao(Id, marcadorCombustivel, quilometragemPercorrida);

        return Devolucao;
    }

    public DetalhamentoValorParcialAluguel CalcularValorParcialDetalhado(
        PlanoCobranca planoSelecionado,
        int quilometragemPercorrida = 0
    )
    {
        var quantidadeDiasPercorridos = ObterQuantidadeDeDiasPercorridos();

        decimal valorPlano = planoSelecionado.CalcularValor(
            quantidadeDiasPercorridos,
            quilometragemPercorrida,
            TipoPlano
        );

        decimal valorTaxas = 0m;

        if (TaxasSelecionadas.Count > 0)
            valorTaxas = TaxasSelecionadas.Sum(tx => tx.CalcularValor(quantidadeDiasPercorridos));

        decimal valorParcial = valorPlano + valorTaxas;

        return new DetalhamentoValorParcialAluguel(
            quantidadeDiasPercorridos,
            valorPlano,
            valorTaxas,
            valorParcial
        );
    }

    public DetalhamentoValorTotalAluguel CalcularValorTotalDetalhado(
        PlanoCobranca planoCobranca,
        MarcadorCombustivel marcadorCombustivel,
        int quilometragemPercorrida
    )
    {
        if (ConfiguracaoCombustiveis is null)
            throw new ArgumentException("A configuração de combustíveis é obrigatória para esta operação.");

        if (Veiculo is null)
            throw new ArgumentException("O veículo é obrigatório para esta operação.");

        var parcial = CalcularValorParcialDetalhado(planoCobranca, quilometragemPercorrida);

        decimal totalAbastecimento;

        decimal valorCombustivel = ConfiguracaoCombustiveis.ObterValorCombustivel(Veiculo.TipoCombustivel);

        totalAbastecimento = Veiculo.CalcularLitrosParaAbastecimento(marcadorCombustivel) * valorCombustivel;

        decimal totalAntesMulta = parcial.ValorParcial + totalAbastecimento;
        
        decimal valorMulta = 0m;

        if (TemMulta()) // Multa (10%) apenas se tiver multa
            valorMulta = totalAntesMulta * (10m / 100m);

        decimal valorTotal = totalAntesMulta + valorMulta;

        return new DetalhamentoValorTotalAluguel(
            parcial.QuantidadeDias,
            parcial.ValorPlano,
            parcial.ValorTaxas,
            parcial.ValorParcial,
            totalAbastecimento,
            valorMulta,
            valorTotal
        );
    }

    private bool TemMulta()
    {
        if (Devolucao is null)
            return (DateTime.UtcNow - DevolucaoPrevistaEmUtc).Days > 0;

        return (Devolucao.OcorrenciaEmUtc - DevolucaoPrevistaEmUtc).Days > 0;
    }

    private int ObterQuantidadeDeDiasPercorridos()
    {
        int qtdDiasLocacao;

        if (Devolucao is null)
            qtdDiasLocacao = (DevolucaoPrevistaEmUtc.Date - InicioEmUtc.Date).Days;
        else
            qtdDiasLocacao = (Devolucao.OcorrenciaEmUtc - InicioEmUtc).Days;

        return qtdDiasLocacao;
    }
}
