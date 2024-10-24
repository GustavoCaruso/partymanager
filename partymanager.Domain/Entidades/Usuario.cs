using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Domain.Entidades
{
    public class Usuario : BaseEntity
    {
        // Nome do usuário
        public string nome { get; set; }

        // Email do usuário
        public string email { get; set; }

        // Senha do usuário (armazenada de forma segura, ex: hash)
        public string senha { get; set; }


        // Relacionamento com os eventos que o usuário criou
        public virtual ICollection<Evento> evento { get; set; } = new List<Evento>();
    }

}

