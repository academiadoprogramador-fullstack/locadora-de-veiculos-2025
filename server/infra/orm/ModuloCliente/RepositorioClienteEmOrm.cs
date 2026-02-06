using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;

public class RepositorioClienteEmOrm(AppDbContext dbContext) :
    RepositorioBaseEmOrm<Cliente>(dbContext)
{

    public override async Task<Cliente?> SelecionarPorIdAsync(Guid id)
    {
        return await registros
            .Include(u => u.Empresa)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public override async Task<List<Cliente>> SelecionarTodosAsync()
    {
        return await registros
            .Include(u => u.Empresa)
            .ToListAsync();
    }
}
