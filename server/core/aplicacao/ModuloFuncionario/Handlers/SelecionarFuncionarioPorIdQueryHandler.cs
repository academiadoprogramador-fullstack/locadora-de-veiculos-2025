using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Compartilhado;
using LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Commands;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.Infraestrutura.Orm.ModuloFuncionario;
using MediatR;

namespace LocadoraDeVeiculos.Aplicacao.ModuloFuncionario.Handlers;

public class SelecionarFuncionarioPorIdQueryHandler(RepositorioFuncionarioEmOrm repositorioFuncionario)
    : IRequestHandler<SelecionarFuncionarioPorIdQuery, Result<SelecionarFuncionarioPorIdResult>>
{
    public async Task<Result<SelecionarFuncionarioPorIdResult>> Handle(
        SelecionarFuncionarioPorIdQuery query, CancellationToken cancellationToken)
    {
        Funcionario? registroEncontrado = await repositorioFuncionario.SelecionarPorIdAsync(query.Id);

        if (registroEncontrado is null)
            return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

        SelecionarFuncionarioPorIdResult result = new(
            registroEncontrado.Id,
            registroEncontrado.NomeCompleto,
            registroEncontrado.Email,
            registroEncontrado.Salario,
            registroEncontrado.AdmissaoEmUtc
        );

        return Result.Ok(result);
    }
}
