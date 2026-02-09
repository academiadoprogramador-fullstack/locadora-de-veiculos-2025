using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Commands;

namespace LocadoraDeVeiculos.Aplicacao.ModuloTaxa.Validators;

public sealed class EditarTaxaCommandValidator
    : AbstractValidator<EditarTaxaCommand>
{
    public EditarTaxaCommandValidator()
    {
        RuleFor(t => t.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório")
            .DependentRules(() =>
            {
                RuleFor(t => t.Nome)
                    .MinimumLength(3)
                    .WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres");
            });

        RuleFor(t => t.Valor)
            .GreaterThan(0)
            .WithMessage("O campo {PropertyName} deve conter um valor maior que 0");

        RuleFor(t => t.TipoCobranca)
            .IsInEnum()
            .WithMessage("O campo {PropertyName} deve conter um valor válido");
    }
}