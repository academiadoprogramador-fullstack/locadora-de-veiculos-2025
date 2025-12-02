using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Infraestrutura.Jwt.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.Handlers;

public class AutenticarUsuarioCommandHandler(
    UserManager<Usuario> userManager,
    AccessTokenProvider tokenProvider,
    RefreshTokenProvider refreshTokenProvider,
    ILogger<AutenticarUsuarioCommandHandler> logger
) : IRequestHandler<AutenticarUsuarioCommand, Result<(AccessToken, RefreshToken)>>
{
    public async Task<Result<(AccessToken, RefreshToken)>> Handle(
        AutenticarUsuarioCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var usuarioEncontrado = await userManager.FindByEmailAsync(command.Email);

            if (usuarioEncontrado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro("Não foi possível encontrar o usuário requisitado."));

            var credenciaisValidas = await userManager.CheckPasswordAsync(
                usuarioEncontrado,
                command.Senha
            );

            if (!credenciaisValidas)
                return Result.Fail(ResultadosErro.RequisicaoInvalidaErro("Login ou senha incorretos."));

            var accessToken = await tokenProvider.GerarAccessTokenAsync(usuarioEncontrado);

            var refreshToken = await refreshTokenProvider.GerarRefreshTokenAsync(usuarioEncontrado);

            return Result.Ok((accessToken, refreshToken));
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a autenticação de {@Command}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
