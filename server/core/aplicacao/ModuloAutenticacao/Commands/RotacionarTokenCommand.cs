using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.Commands;

public record RotacionarTokenCommand(string RefreshTokenString)
    : IRequest<Result<(AccessToken AccessToken, RefreshToken RefreshToken)>>;
