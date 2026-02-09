using LocadoraDeVeiculos.Dominio.ModuloCombustivel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCombustivel;

public sealed class MapeadorConfiguracaoCombustiveisEmOrm : IEntityTypeConfiguration<ConfiguracaoCombustiveis>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoCombustiveis> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(c => c.CriadaEm)
               .HasColumnType("datetimeoffset")
               .IsRequired();

        builder.Property(c => c.ValorAlcool)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(c => c.ValorDiesel)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(c => c.ValorEletricidade)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(c => c.ValorGas)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(c => c.ValorGasolina)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(c => c.Empresa)
               .WithMany()
               .HasForeignKey(f => f.EmpresaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => f.EmpresaId);
    }
}
