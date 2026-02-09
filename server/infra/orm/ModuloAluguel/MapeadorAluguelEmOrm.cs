using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloAluguel;

public sealed class MapeadorAluguelEmOrm : IEntityTypeConfiguration<Aluguel>
{
    public void Configure(EntityTypeBuilder<Aluguel> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.TipoPlano)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(a => a.Status)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(a => a.InicioEmUtc)
            .HasColumnType("datetimeoffset")
            .IsRequired();

        builder.Property(a => a.DevolucaoPrevistaEmUtc)
            .HasColumnType("datetimeoffset")
            .IsRequired();

        builder.HasOne(a => a.Condutor)
            .WithMany()
            .HasForeignKey(a => a.CondutorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Veiculo)
            .WithMany()
            .HasForeignKey(a => a.VeiculoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.ConfiguracaoCombustiveis)
            .WithMany()
            .HasForeignKey(a => a.ConfiguracaoCombustiveisId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.TaxasSelecionadas)
            .WithMany();

        builder.HasOne(a => a.Devolucao)
            .WithOne()
            .HasForeignKey<Devolucao>(d => d.AluguelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Empresa)
            .WithMany()
            .HasForeignKey(a => a.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.EmpresaId, a.Excluido });
        builder.HasIndex(a => new { a.EmpresaId, a.Excluido, a.Status });
    }
}
