using FluentResults;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;

public record SelecionarCondutoresPorClienteIdQuery(Guid ClienteId)
    : IRequest<Result<SelecionarCondutoresPorClienteIdResult>>;

public record SelecionarCondutoresPorClienteIdResult(
    IReadOnlyList<SelecionarCondutoresDto> Registros
);