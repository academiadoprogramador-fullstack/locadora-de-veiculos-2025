using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCondutor;

public class MapeadorCondutorEmOrm : IEntityTypeConfiguration<Condutor>
{
    public void Configure(EntityTypeBuilder<Condutor> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(c => c.ClienteCondutor)
               .HasColumnType("bit")
               .IsRequired();

        builder.Property(c => c.Nome)
               .HasColumnType("nvarchar(100)")
               .IsRequired();

        builder.Property(c => c.Email)
               .HasColumnType("varchar(100)")
               .IsRequired();

        builder.Property(c => c.Telefone)
               .HasColumnType("varchar(20)")
               .IsRequired();

        builder.Property(c => c.Cpf)
               .HasColumnType("varchar(14)")
               .IsRequired();

        builder.Property(c => c.Cnh)
               .HasColumnType("varchar(20)")
               .IsRequired();

        builder.Property(c => c.ValidadeCnh)
               .HasColumnType("datetimeoffset")
               .IsRequired();

        builder.HasOne(c => c.Cliente)
               .WithMany(cl => cl.Condutores)
               .HasForeignKey(c => c.ClienteId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Empresa)
               .WithMany()
               .HasForeignKey(f => f.EmpresaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => new { f.EmpresaId, f.Excluido });
    }
}
