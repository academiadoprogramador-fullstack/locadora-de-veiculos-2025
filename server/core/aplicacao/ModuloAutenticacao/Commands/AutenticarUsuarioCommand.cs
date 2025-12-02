using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.Commands;

public record AutenticarUsuarioCommand(string Email, string Senha)
    : IRequest<Result<(AccessToken AccessToken, RefreshToken RefreshToken)>>;