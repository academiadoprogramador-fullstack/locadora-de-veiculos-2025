using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;

public record ExcluirPlanoCobrancaCommand(Guid Id) : IRequest<Result<ExcluirPlanoCobrancaResult>>;

public record ExcluirPlanoCobrancaResult();
