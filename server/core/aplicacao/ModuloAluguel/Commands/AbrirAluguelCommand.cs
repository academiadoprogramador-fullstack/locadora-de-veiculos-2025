using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

public record AbrirAluguelCommand(Guid Id)
    : IRequest<Result<AbrirAluguelResult>>;

public record AbrirAluguelResult();
