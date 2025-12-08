using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Commands;

namespace LocadoraDeVeiculos.Aplicacao.ModuloGrupoVeiculos.Validators;

public class CadastrarGrupoVeiculosCommandValidator 
    : AbstractValidator<CadastrarGrupoVeiculosCommand>
{
    public CadastrarGrupoVeiculosCommandValidator()
    {
        RuleFor(m => m.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório")
            .DependentRules(() =>
            {
                RuleFor(m => m.Nome).MinimumLength(3)
                    .WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres");
            });
    }
}