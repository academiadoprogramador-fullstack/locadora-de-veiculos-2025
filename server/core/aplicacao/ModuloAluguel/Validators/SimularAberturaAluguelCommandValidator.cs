using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Commands;

namespace LocadoraDeVeiculos.Aplicacao.ModuloAluguel.Validators;

public sealed class SimularAberturaAluguelCommandValidator : AbstractValidator<SimularAberturaAluguelCommand>
{
    public SimularAberturaAluguelCommandValidator()
    {
        RuleFor(m => m.CondutorId)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.VeiculoId)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.ConfiguracaoCombustiveisId)
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

        // Taxas são opcionais, mas se vierem, não podem ter Guid vazio
        RuleForEach(m => m.TaxasSelecionadasIds)
            .NotEmpty().WithMessage("O campo TaxasSelecionadasIds não pode conter um id vazio");
    }
}
