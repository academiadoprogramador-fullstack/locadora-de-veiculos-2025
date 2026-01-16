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

public class CadastrarPlanoCobrancaCommandHandler(
    AppDbContext appDbContext,
    RepositorioPlanoCobrancaEmOrm repositorioPlanoCobranca,
    ITenantProvider tenantProvider,
    ILogger<CadastrarPlanoCobrancaCommandHandler> logger
) : IRequestHandler<CadastrarPlanoCobrancaCommand, Result<CadastrarPlanoCobrancaResult>>
{
    public async Task<Result<CadastrarPlanoCobrancaResult>> Handle(
        CadastrarPlanoCobrancaCommand command,
        CancellationToken cancellationToken
    )
    {
        List<PlanoCobranca> registros = await repositorioPlanoCobranca.SelecionarTodosAsync();

        if (registros.Any(x => x.GrupoVeiculosId.Equals(command.GrupoVeiculosId)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Um plano de cobrança para este grupo de veículos já existe."));

        try
        {
            PlanoCobranca planoCobranca = new(
                tenantProvider.EmpresaId.GetValueOrDefault(),
                command.GrupoVeiculosId,
                command.PrecosPlanoDiario.PrecoDiarioPlanoDiario,
                command.PrecosPlanoDiario.PrecoQuilometroPlanoDiario,
                command.PrecosPlanoControlado.QuilometrosDisponiveisPlanoControlado,
                command.PrecosPlanoControlado.PrecoDiarioPlanoControlado,
                command.PrecosPlanoControlado.PrecoQuilometroExtrapoladoPlanoControlado,
                command.PrecosPlanoLivre.PrecoDiarioPlanoLivre
            );

            await repositorioPlanoCobranca.CadastrarAsync(planoCobranca);

            await appDbContext.SaveChangesAsync(cancellationToken);

            CadastrarPlanoCobrancaResult result = new(planoCobranca.Id);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante o cadastro de {@Command}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
