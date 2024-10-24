using FluentValidation;
using partymanager.Domain.Entidades;

namespace partymanager.Service.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            // Validação para o nome do usuário
            RuleFor(u => u.nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome pode ter no máximo 100 caracteres.");

            // Validação para o email do usuário
            RuleFor(u => u.email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("Formato de email inválido.")
                .MaximumLength(100).WithMessage("O email pode ter no máximo 100 caracteres.");

            // Validação para a senha do usuário
            RuleFor(u => u.senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
                .Matches(@"[A-Z]+").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
                .Matches(@"[a-z]+").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
                .Matches(@"\d+").WithMessage("A senha deve conter pelo menos um número.")
                .Matches(@"[\W_]+").WithMessage("A senha deve conter pelo menos um caractere especial (ex: @, #, $, etc).");
        }
    }
}
