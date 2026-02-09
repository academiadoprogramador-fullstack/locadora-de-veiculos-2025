using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Commands;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCondutor.Validators;

public sealed class CadastrarCondutorCommandValidator
    : AbstractValidator<CadastrarCondutorCommand>
{
    public CadastrarCondutorCommandValidator()
    {
        RuleFor(m => m.ClienteId)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório")
            .DependentRules(() =>
            {
                RuleFor(m => m.Nome)
                    .MinimumLength(3)
                    .WithMessage("O campo {PropertyName} deve conter no mínimo {MinLength} caracteres");
            });

        RuleFor(m => m.Email)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório")
            .EmailAddress().WithMessage("O campo {PropertyName} deve conter um e-mail válido");

        RuleFor(m => m.Telefone)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.Cpf)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        // CPF com máscara 000.000.000-00 (ou sem máscara 00000000000)
        RuleFor(m => m.Cpf)
            .Matches(@"^(\d{3}\.\d{3}\.\d{3}-\d{2}|\d{11})$")
            .WithMessage("O campo {PropertyName} deve conter um CPF válido");

        RuleFor(m => m.Cnh)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        // CNH (regra simples): 11 dígitos
        RuleFor(m => m.Cnh)
            .Matches(@"^\d{11}$")
            .WithMessage("O campo {PropertyName} deve conter uma CNH válida");

        RuleFor(m => m.ValidadeCnh)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("O campo {PropertyName} deve conter uma data maior que a data atual");
    }
}