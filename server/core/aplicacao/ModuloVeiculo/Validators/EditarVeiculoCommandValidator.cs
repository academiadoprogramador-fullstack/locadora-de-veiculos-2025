using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Commands;

namespace LocadoraDeVeiculos.Aplicacao.ModuloVeiculo.Validators;

public sealed class EditarVeiculoCommandValidator : AbstractValidator<EditarVeiculoCommand>
{
    public EditarVeiculoCommandValidator()
    {
        RuleFor(m => m.GrupoVeiculosId)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.Modelo)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório")
            .DependentRules(() =>
            {
                RuleFor(m => m.Modelo).MinimumLength(3)
                    .WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres");
            });

        RuleFor(m => m.Marca)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.Ano)
            .GreaterThan(2000).WithMessage("O campo {PropertyName} deve conter um valor maior que 2000");

        RuleFor(m => m.CapacidadeTanque)
            .GreaterThan(0).WithMessage("O campo {PropertyName} deve conter um valor maior que 0");

        RuleFor(m => m.TipoCombustivel)
            .IsInEnum().WithMessage("O campo {PropertyName} deve conter um valor válido");
    }
}