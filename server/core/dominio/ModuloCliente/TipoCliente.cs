using System.ComponentModel.DataAnnotations;

namespace LocadoraDeVeiculos.Dominio.ModuloCliente;

public enum TipoCliente
{
    [Display(Name = "Pessoa Física")] Cpf,
    [Display(Name = "Pessoa Jurídica")] Cnpj
}
