using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Commands;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloCombustivel;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCombustivel.Handlers;

public sealed class SelecionarUltimaConfiguracaoCombustiveisQueryHandler(
    RepositorioConfiguracaoCombustiveisEmOrm repositorioConfiguracao
) : IRequestHandler<SelecionarUltimaConfiguracaoCombustiveisQuery, Result<SelecionarUltimaConfiguracaoCombustiveisResult>>
{
    public async Task<Result<SelecionarUltimaConfiguracaoCombustiveisResult>> Handle(
        SelecionarUltimaConfiguracaoCombustiveisQuery request,
        CancellationToken cancellationToken
    )
    {
        var registro = await repositorioConfiguracao.SelecionarUltimaConfiguracao();

        if (registro is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(Guid.Empty));

        var response = new SelecionarUltimaConfiguracaoCombustiveisResult(
            registro.Id,
            registro.CriadaEm,
            registro.ValorAlcool,
            registro.ValorDiesel,
            registro.ValorEletricidade,
            registro.ValorGas,
            registro.ValorGasolina
        );

        return Result.Ok(response);
    }
}