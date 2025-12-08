using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;

public class RepositorioGrupoVeiculosEmOrm : RepositorioBaseEmOrm<GrupoVeiculos>
{
    private readonly AppDbContext dbContext;

    public RepositorioGrupoVeiculosEmOrm(AppDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }
}
