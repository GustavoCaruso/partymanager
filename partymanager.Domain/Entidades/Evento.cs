using System;
using System.Collections.Generic;

namespace partymanager.Domain.Entidades
{
    public class Evento : BaseEntity
    {
        
        public string nome { get; set; }
 
        public DateTime data { get; set; }
        public string localidade { get; set; }
        public int idTipoEvento { get; set; }

        public virtual TipoEvento tipoevento { get; set; }

        public int numeroAdultos { get; set; }

        public int numeroCriancas { get; set; }



        public virtual ICollection<ItemCalculado> itemcalculado { get; set; } = new List<ItemCalculado>();

        public int? idUsuario { get; set; }
        public virtual Usuario usuario { get; set; }  // Caso deseje associar o evento a um usuário
    }
}
