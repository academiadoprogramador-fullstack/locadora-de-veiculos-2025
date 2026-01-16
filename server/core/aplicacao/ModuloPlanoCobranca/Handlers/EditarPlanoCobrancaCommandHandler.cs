using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Commands;
using LocadoraDeVeiculos.Dominio.ModuloAutenticacao;
using LocadoraDeVeiculos.Dominio.ModuloPlanoCobranca;
using LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloPlanoCobranca;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LocadoraDeVeiculos.Aplicacao.ModuloPlanoCobranca.Handlers;

public class EditarPlanoCobrancaCommandHandler(
    AppDbContext appDbContext,
    RepositorioPlanoCobrancaEmOrm repositorioPlanoCobranca,
    ITenantProvider tenantProvider,
    ILogger<EditarPlanoCobrancaCommandHandler> logger
)
    : IRequestHandler<EditarPlanoCobrancaCommand, Result<EditarPlanoCobrancaResult>>
{
    public async Task<Result<EditarPlanoCobrancaResult>> Handle(
        EditarPlanoCobrancaCommand command,
        CancellationToken cancellationToken
    )
    {
        PlanoCobranca? registroEncontrado = await repositorioPlanoCobranca.SelecionarPorIdAsync(command.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

        try
        {
            PlanoCobranca grupoVeiculosEditado = new(
                tenantProvider.EmpresaId.GetValueOrDefault(),
                registroEncontrado.GrupoVeiculosId,
                command.PrecosPlanoDiario.PrecoDiarioPlanoDiario,
                command.PrecosPlanoDiario.PrecoQuilometroPlanoDiario,
                command.PrecosPlanoControlado.QuilometrosDisponiveisPlanoControlado,
                command.PrecosPlanoControlado.PrecoDiarioPlanoControlado,
                command.PrecosPlanoControlado.PrecoQuilometroExtrapoladoPlanoControlado,
                command.PrecosPlanoLivre.PrecoDiarioPlanoLivre
            );

            await repositorioPlanoCobranca.EditarAsync(command.Id, grupoVeiculosEditado);

            await appDbContext.SaveChangesAsync(cancellationToken);

            EditarPlanoCobrancaResult result = new(
                new PlanoDiarioDto(
                    command.PrecosPlanoDiario.PrecoDiarioPlanoDiario,
                    command.PrecosPlanoDiario.PrecoQuilometroPlanoDiario
                ),
                new PlanoControladoDto(
                    command.PrecosPlanoControlado.QuilometrosDisponiveisPlanoControlado,
                    command.PrecosPlanoControlado.PrecoDiarioPlanoControlado,
                    command.PrecosPlanoControlado.PrecoQuilometroExtrapoladoPlanoControlado
                ),
                new PlanoLivreDto(command.PrecosPlanoLivre.PrecoDiarioPlanoLivre)
            );

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Command}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
