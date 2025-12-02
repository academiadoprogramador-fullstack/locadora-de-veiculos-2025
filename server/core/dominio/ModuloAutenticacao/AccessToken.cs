namespace LocadoraDeVeiculos.Dominio.ModuloAutenticacao;

public record AccessToken(
    string Chave,
    DateTime Expiracao,
    UsuarioAutenticado UsuarioAutenticado
);