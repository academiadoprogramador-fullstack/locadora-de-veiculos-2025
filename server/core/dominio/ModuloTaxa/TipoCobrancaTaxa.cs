using System.ComponentModel.DataAnnotations;

namespace LocadoraDeVeiculos.Dominio.ModuloTaxa;

public enum TipoCobrancaTaxa
{
    [Display(Name = "Diária")]
    Diaria,
    Fixa
}
