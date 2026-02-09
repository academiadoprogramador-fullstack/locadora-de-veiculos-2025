using LocadoraDeVeiculos.Dominio.ModuloCombustivel;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCombustivel;

public sealed class RepositorioConfiguracaoCombustiveisEmOrm(AppDbContext dbContext)
{
    public async Task Cadastrar(ConfiguracaoCombustiveis configuracao)
    {
        await dbContext.ConfiguracoesCombustiveis.AddAsync(configuracao);
    }

    public async Task<List<ConfiguracaoCombustiveis>> SelecionarConfiguracoesEmOrdemDescendente()
    {
        return await dbContext.ConfiguracoesCombustiveis
            .Include(c => c.Empresa)
            .OrderByDescending(c => c.CriadaEm)
            .ToListAsync();
    }

    public async Task<ConfiguracaoCombustiveis?> SelecionarUltimaConfiguracao()
    {
        return await dbContext.ConfiguracoesCombustiveis
            .Include(c => c.Empresa)
            .OrderByDescending(c => c.CriadaEm)
            .FirstOrDefaultAsync();
    }
}