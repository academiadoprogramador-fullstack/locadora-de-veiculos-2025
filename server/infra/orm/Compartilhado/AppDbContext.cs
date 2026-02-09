using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCombustivel;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Dominio.ModuloGrupoVeiculos;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
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
    public DbSet<GrupoVeiculos> GruposVeiculos { get; set; }
    public DbSet<PlanoCobranca> PlanosCobranca { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Condutor> Condutores { get; set; }
    public DbSet<Taxa> Taxas { get; set; }
    public DbSet<ConfiguracaoCombustiveis> ConfiguracoesCombustiveis { get; set; }
    public DbSet<Aluguel> Alugueis { get; set; }
    public DbSet<Devolucao> Devolucoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (tenantProvider is not null)
        {
            // Query Filters
            modelBuilder.Entity<Funcionario>()
                        .HasQueryFilter(f => f.EmpresaId == tenantProvider.EmpresaId.GetValueOrDefault() && !f.Excluido);

            modelBuilder.Entity<GrupoVeiculos>()
                       .HasQueryFilter(f => f.EmpresaId == tenantProvider.EmpresaId.GetValueOrDefault() && !f.Excluido);

            modelBuilder.Entity<PlanoCobranca>()
                       .HasQueryFilter(f => f.EmpresaId == tenantProvider.EmpresaId.GetValueOrDefault() && !f.Excluido);

            modelBuilder.Entity<Veiculo>()
                .HasQueryFilter(f => f.EmpresaId == tenantProvider.EmpresaId.GetValueOrDefault() && !f.Excluido);

            modelBuilder.Entity<Cliente>()
                .HasQueryFilter(f => !f.Excluido && f.EmpresaId == tenantProvider.EmpresaId.GetValueOrDefault());

            modelBuilder.Entity<Condutor>()
                .HasQueryFilter(f => !f.Excluido && f.EmpresaId == tenantProvider.EmpresaId.GetValueOrDefault());

            modelBuilder.Entity<Taxa>()
                .HasQueryFilter(f => !f.Excluido && f.EmpresaId == tenantProvider.EmpresaId.GetValueOrDefault());

            modelBuilder.Entity<ConfiguracaoCombustiveis>()
                .HasQueryFilter(f => f.EmpresaId == tenantProvider.EmpresaId.GetValueOrDefault());

            modelBuilder.Entity<Aluguel>()
                .HasQueryFilter(f => !f.Excluido && f.EmpresaId == tenantProvider.EmpresaId.GetValueOrDefault());
        }

        var assembly = typeof(AppDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
}
