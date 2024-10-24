using FluentValidation;
using partymanager.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Service.Validators
{
    public class TipoEventoValidator : AbstractValidator<TipoEvento>
    {
        public TipoEventoValidator()
        {
            // Nome não pode ser vazio
            RuleFor(e => e.nome)
                .NotEmpty().WithMessage("Informe o nome do Tipo do Evento!")
                .NotNull().WithMessage("Informe o nome do Tipo do Evento!");
        }
    }
}
