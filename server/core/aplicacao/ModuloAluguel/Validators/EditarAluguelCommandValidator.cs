using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Validators;

public sealed class EditarAluguelCommandValidator : AbstractValidator<EditarAluguelCommand>
{
    public EditarAluguelCommandValidator()
    {
        RuleFor(m => m.Id)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.CondutorId)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.VeiculoId)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.TipoPlano)
            .IsInEnum().WithMessage("O campo {PropertyName} deve conter um valor válido");

        RuleFor(m => m.InicioEmUtc)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.DevolucaoPrevistaEmUtc)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório")
            .DependentRules(() =>
            {
                RuleFor(m => m.DevolucaoPrevistaEmUtc)
                    .GreaterThan(m => m.InicioEmUtc)
                    .WithMessage("O campo {PropertyName} deve ser maior que {ComparisonProperty}");
            });

        RuleForEach(m => m.TaxasSelecionadasIds)
            .NotEmpty().WithMessage("O campo TaxasSelecionadasIds não pode conter um id vazio");
    }
}