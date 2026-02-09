using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCondutor;

public class RepositorioCondutorEmOrm(AppDbContext dbContext) :
    RepositorioBaseEmOrm<Condutor>(dbContext)
{
    public async Task<List<Condutor>> SelecionarCondutoresPorIdClienteAsync(Guid clienteId)
    {
        return await registros
            .Include(u => u.Empresa)
            .Include(u => u.Cliente)
            .Where(c => c.ClienteId == clienteId)
            .ToListAsync();
    }

    public override async Task<Condutor?> SelecionarPorIdAsync(Guid id)
    {
        return await registros
            .Include(u => u.Empresa)
            .Include(u => u.Cliente)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public override async Task<List<Condutor>> SelecionarTodosAsync()
    {
        return await registros
            .Include(u => u.Empresa)
            .Include(u => u.Cliente)
            .ToListAsync();
    }
}
