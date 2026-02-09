using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;

public class RepositorioAluguelEmOrm(AppDbContext dbContext) : RepositorioBaseEmOrm<Aluguel>(dbContext)
{
    public override async Task<Aluguel?> SelecionarPorIdAsync(Guid idRegistro)
    {
        return await registros
            .Include(a => a.Condutor)
            .Include(a => a.ConfiguracaoCombustiveis)
            .Include(a => a.Veiculo)
            .Include(a => a.TaxasSelecionadas)
            .Include(a => a.Empresa)
            .Include(a => a.Devolucao)
            .FirstOrDefaultAsync(a => a.Id == idRegistro);
    }

    public override async Task<List<Aluguel>> SelecionarTodosAsync()
    {
        return await registros
            .Include(a => a.Condutor)
            .Include(a => a.Veiculo)
            .Include(a => a.Empresa)
            .Include(a => a.Devolucao)
            .ToListAsync();
    }
}
