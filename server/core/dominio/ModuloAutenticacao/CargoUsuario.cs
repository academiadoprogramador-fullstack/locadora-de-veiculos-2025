using System.ComponentModel.DataAnnotations;

namespace LocadoraDeVeiculos.Dominio.ModuloAutenticacao;

public enum CargoUsuario
{
    Empresa,
    [Display( Name = "Funcionário" )] Funcionario
}
