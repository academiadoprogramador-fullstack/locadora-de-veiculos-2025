using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxa;

public class RepositorioTaxaEmOrm(AppDbContext dbContext) : RepositorioBaseEmOrm<Taxa>(dbContext)
{
    public override async Task<Taxa?> SelecionarPorIdAsync(Guid id)
    {
        return await registros
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public override async Task<List<Taxa>> SelecionarTodosAsync()
    {
        return await registros
            .Include(u => u.Empresa)
            .ToListAsync();
    }

    public async Task<List<Taxa>> SelecionarMuitosPorIdsAsync(IEnumerable<Guid> ids)
    {
        return await registros
            .Include(t => t.Empresa)
            .Where(t => ids.Contains(t.Id))
            .ToListAsync();
    }
}
