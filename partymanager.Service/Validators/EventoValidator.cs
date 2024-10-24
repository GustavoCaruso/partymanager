using FluentValidation;
using partymanager.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Service.Validators
{
    public class EventoValidator : AbstractValidator<Evento>
    {
        public EventoValidator()
        {
           
        }
    }
}
