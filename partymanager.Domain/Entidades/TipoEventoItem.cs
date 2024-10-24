using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Domain.Entidades
{
    public class TipoEventoItem : BaseEntity
    {
        public int idTipoEvento { get; set; }
        public virtual TipoEvento tipoevento { get; set; }

        public int idItem { get; set; }
        public virtual Item item { get; set; }

        public decimal quantidadePorPessoa { get; set; }  // Define a quantidade de itens por pessoa
        
    }
}
