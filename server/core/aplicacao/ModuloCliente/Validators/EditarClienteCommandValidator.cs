using FluentValidation;
using LocadoraDeVeiculos.Aplicacao.ModuloCliente.Commands;
using LocadoraDeVeiculos.Dominio.ModuloCliente;

namespace LocadoraDeVeiculos.Aplicacao.ModuloCliente.Validators;

public sealed class EditarClienteCommandValidator
    : AbstractValidator<EditarClienteCommand>
{
    public EditarClienteCommandValidator()
    {
        RuleFor(m => m.Id)
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

        RuleFor(m => m.Tipo)
            .IsInEnum().WithMessage("O campo {PropertyName} deve conter um valor válido");

        RuleFor(m => m.NumeroDocumento)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        // CPF: 000.000.000-00 (ou 00000000000)
        When(m => m.Tipo == TipoCliente.Cpf, () =>
        {
            RuleFor(m => m.NumeroDocumento)
                .Matches(@"^(\d{3}\.\d{3}\.\d{3}-\d{2}|\d{11})$")
                .WithMessage("O campo {PropertyName} deve conter um CPF válido");
        });

        // CNPJ: 00.000.000/0000-00 (ou 00000000000000)
        When(m => m.Tipo == TipoCliente.Cnpj, () =>
        {
            RuleFor(m => m.NumeroDocumento)
                .Matches(@"^(\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}|\d{14})$")
                .WithMessage("O campo {PropertyName} deve conter um CNPJ válido");
        });


        RuleFor(m => m.Cidade)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.Estado)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.Bairro)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.Rua)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        RuleFor(m => m.Numero)
            .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
    }
}