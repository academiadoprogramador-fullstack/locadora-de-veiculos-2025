using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculo;

public sealed class RepositorioVeiculoEmOrm(AppDbContext contexto) : RepositorioBaseEmOrm<Veiculo>(contexto)
{
    public override async Task<Veiculo?> SelecionarPorIdAsync(Guid idRegistro)
    {
        return await registros
                    .Include(v => v.GrupoVeiculos)
                    .Include(v => v.Empresa)
                    .FirstOrDefaultAsync(v => v.Id == idRegistro);
    }

    public override async Task<List<Veiculo>> SelecionarTodosAsync()
    {
        return await registros
                    .Include(v => v.GrupoVeiculos)
                    .Include(v => v.Empresa)
                    .ToListAsync();
    }
}
