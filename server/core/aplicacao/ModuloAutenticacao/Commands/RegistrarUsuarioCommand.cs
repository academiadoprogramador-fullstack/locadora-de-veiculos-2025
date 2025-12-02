using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.Commands;

public record RegistrarUsuarioCommand(
    string NomeCompleto,
    string Email,
    string Senha,
    string ConfirmarSenha
) : IRequest<Result<(AccessToken AccessToken, RefreshToken RefreshToken)>>;
