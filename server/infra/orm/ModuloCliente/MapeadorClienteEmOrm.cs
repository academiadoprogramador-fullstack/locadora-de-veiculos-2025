using LocadoraDeVeiculos.Dominio.ModuloCliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCliente;

public class MapeadorClienteEmOrm : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(c => c.Nome)
               .HasColumnType("nvarchar(100)")
               .IsRequired();

        builder.Property(c => c.Email)
               .HasColumnType("varchar(100)")
               .IsRequired();

        builder.Property(c => c.Telefone)
               .HasColumnType("varchar(20)")
               .IsRequired();

        builder.Property(c => c.Tipo)
               .HasColumnType("int")
               .IsRequired();

        builder.Property(c => c.NumeroDocumento)
               .HasColumnType("varchar(25)")
               .IsRequired();

        builder.Property(c => c.Cidade)
               .HasColumnType("varchar(25)")
               .IsRequired();

        builder.Property(c => c.Estado)
               .HasColumnType("nvarchar(80)")
               .IsRequired();

        builder.Property(c => c.Bairro)
               .HasColumnType("nvarchar(80)")
               .IsRequired();

        builder.Property(c => c.Rua)
               .HasColumnType("varchar(100)")
               .IsRequired();

        builder.Property(c => c.Numero)
               .HasColumnType("varchar(20)")
               .IsRequired();

        builder.HasOne(c => c.Empresa)
               .WithMany()
               .HasForeignKey(f => f.EmpresaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => new { f.EmpresaId, f.Excluido });
    }
}
