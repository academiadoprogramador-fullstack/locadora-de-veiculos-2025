using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.ModuloFuncionario;

public class MapeadorFuncionarioEmOrm : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(c => c.NomeCompleto)
               .HasColumnType("nvarchar(100)")
               .IsRequired();

        builder.Property(c => c.Cpf)
               .HasColumnType("varchar(14)")
               .IsRequired();

        builder.Property(c => c.Email)
               .HasColumnType("varchar(100)")
               .IsRequired();

        builder.Property(c => c.Salario)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(c => c.AdmissaoEmUtc)
               .HasColumnType("datetimeoffset")
               .IsRequired();

        builder.HasOne(c => c.Usuario)
               .WithOne()
               .HasForeignKey<Funcionario>(f => f.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Empresa)
               .WithMany()
               .HasForeignKey(f => f.EmpresaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(f => new { f.EmpresaId, f.Excluido });
    }
}