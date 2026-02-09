using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloTaxa;

public class MapeadorTaxaEmOrm : IEntityTypeConfiguration<Taxa>
{
    public void Configure(EntityTypeBuilder<Taxa> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(c => c.Nome)
               .HasColumnType("nvarchar(100)")
               .IsRequired();

        builder.Property(c => c.Valor)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(c => c.TipoCobranca)
               .HasColumnType("int")
               .IsRequired();

        builder.HasOne(c => c.Empresa)
               .WithMany()
               .HasForeignKey(f => f.EmpresaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => new { f.EmpresaId, f.Excluido });
    }
}
