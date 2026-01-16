using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;

public class MapeadorPlanoCobrancaEmOrm : IEntityTypeConfiguration<PlanoCobranca>
{
    public void Configure(EntityTypeBuilder<PlanoCobranca> builder)
    {

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PrecoDiarioPlanoDiario)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.PrecoQuilometroPlanoDiario)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.QuilometrosDisponiveisPlanoControlado)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.PrecoDiarioPlanoControlado)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.PrecoQuilometroExtrapoladoPlanoControlado)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.PrecoDiarioPlanoLivre)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(p => p.GrupoVeiculos)
               .WithMany()
               .HasForeignKey(p => p.GrupoVeiculosId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Empresa)
               .WithMany()
               .HasForeignKey(f => f.EmpresaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => new { f.EmpresaId, f.Excluido });
    }
}