using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

public class AppDbContext(
    DbContextOptions options,
    ITenantProvider? tenantProvider = null
) : IdentityDbContext<Usuario, Cargo, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (tenantProvider is not null)
        {
            // Query Filters
            modelBuilder.Entity<Funcionario>()
                        .HasQueryFilter(f => f.EmpresaId == tenantProvider.EmpresaId.GetValueOrDefault() && !f.Excluido);
        }

        var assembly = typeof(AppDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
}
