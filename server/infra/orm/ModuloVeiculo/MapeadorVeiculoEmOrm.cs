using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloVeiculo;

public sealed class MapeadorVeiculoEmOrm : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(c => c.Modelo)
               .HasColumnType("nvarchar(100)")
               .IsRequired();

        builder.Property(c => c.Marca)
               .HasColumnType("nvarchar(100)")
               .IsRequired();

        builder.Property(c => c.Ano)
               .HasColumnType("nvarchar(100)")
               .IsRequired();

        builder.Property(c => c.Imagem)
               .HasColumnType("nvarchar(350)")
               .IsRequired(false);

        builder.Property(c => c.CapacidadeTanque)
               .HasColumnType("decimal(18, 2)")
               .IsRequired();

        builder.Property(c => c.TipoCombustivel)
               .HasColumnType("int")
               .IsRequired();

        builder.HasOne(c => c.GrupoVeiculos)
               .WithMany()
               .HasForeignKey(f => f.GrupoVeiculosId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Empresa)
               .WithMany()
               .HasForeignKey(f => f.EmpresaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => new { f.EmpresaId, f.Excluido });
    }
}