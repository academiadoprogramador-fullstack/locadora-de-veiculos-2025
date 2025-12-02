using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAutenticacao.Commands;

public record SairCommand(string RefreshTokenHash) : IRequest<Result>;