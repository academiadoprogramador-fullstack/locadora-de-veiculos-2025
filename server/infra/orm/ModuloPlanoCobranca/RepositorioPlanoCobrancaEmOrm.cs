using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;


namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;

public class RepositorioPlanoCobrancaEmOrm(AppDbContext contexto) : RepositorioBaseEmOrm<PlanoCobranca>(contexto)
{
    public async Task<PlanoCobranca?> SelecionarPlanoDeCobrancaPorGrupoVeiculosIdAsync(Guid grupoVeiculosId)
    {
        return await registros
            .Include(p => p.GrupoVeiculos)
            .Include(p => p.Empresa)
            .FirstOrDefaultAsync(p => p.GrupoVeiculosId == grupoVeiculosId);
    }

    public override async Task<PlanoCobranca?> SelecionarPorIdAsync(Guid idRegistro)
    {
        return await registros
            .Include(p => p.GrupoVeiculos)
            .Include(p => p.Empresa)
            .FirstOrDefaultAsync(p => p.Id == idRegistro);
    }

    public override async Task<List<PlanoCobranca>> SelecionarTodosAsync()
    {
        return await registros
            .Include(p => p.GrupoVeiculos)
            .Include(p => p.Empresa)
            .ToListAsync();
    }
}
