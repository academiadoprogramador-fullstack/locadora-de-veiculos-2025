using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloFuncionario;

public class RepositorioFuncionarioEmOrm(AppDbContext dbContext) : RepositorioBaseEmOrm<Funcionario>(dbContext)
{
    public override async Task<Funcionario?> SelecionarPorIdAsync(Guid funcionarioId)
    {
        return await registros
            .Include(u => u.Empresa)
            .Include(u => u.Usuario)
            .FirstOrDefaultAsync(f => f.Id == funcionarioId);
    }

    public async Task<Funcionario?> SelecionarPorUsuarioIdAsync(Guid usuarioId)
    {
        return await registros
            .IgnoreQueryFilters()
            .Include(u => u.Empresa)
            .Include(u => u.Usuario)
            .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId);
    }

    public override async Task<List<Funcionario>> SelecionarTodosAsync()
    {
        return await registros
            .Include(u => u.Empresa)
            .Include(u => u.Usuario)
            .ToListAsync();
    }
}