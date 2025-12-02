using Microsoft.AspNetCore.Identity;

namespace LocadoraDeVeiculos.Dominio.ModuloAutenticacao;

public class Usuario : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public Guid AccessTokenVersionId { get; set; } = Guid.Empty;

    public Usuario()
    {
        Id = Guid.NewGuid();
        EmailConfirmed = true;
    }
}

public record UsuarioAutenticado(
    Guid Id,
    string NomeCompleto,
    string Email,
    CargoUsuario Cargo
);