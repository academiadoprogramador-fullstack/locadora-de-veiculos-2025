using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloGrupoVeiculos;

public class RepositorioGrupoVeiculosEmOrm(AppDbContext dbContext) : RepositorioBaseEmOrm<GrupoVeiculos>(dbContext)
{
}
