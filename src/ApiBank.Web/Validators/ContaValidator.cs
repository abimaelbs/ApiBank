using ApiBank.Web.Models;
using FluentValidation;

namespace ApiBank.Web.Validators
{
    public class ContaValidator : AbstractValidator<Conta>
    {
        public ContaValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome não obrigatório.")
                .NotNull().WithMessage("Nome não pode ser nullo")
                .Length(1, 255).WithMessage("O nome deve ter entre 1 e  caracteres");
                
            RuleFor(x => x.Date)
                .NotNull().WithMessage("Data é obrigatório");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Tipo não pode ser vazio")
                .NotNull().WithMessage("Tipo Não pode ser nulo");                
        }
    }
}
