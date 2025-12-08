using LocadoraDeVeiculos.Dominio.Compartilhado;

namespace LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;

public class GrupoVeiculos : EntidadeBase<GrupoVeiculos>
{
    public string Nome { get; set; }

    public GrupoVeiculos(Guid empresaId, string nome)
    {
        EmpresaId = empresaId;
        Nome = nome;
    }

    public override void AtualizarRegistro(GrupoVeiculos registroEditado)
    {
        Nome = registroEditado.Nome;
    }
}
