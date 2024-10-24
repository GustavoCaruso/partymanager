using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Domain.Entidades
{
    public class ItemCalculado : BaseEntity
    {
        public int idEvento { get; set; }
        public virtual Evento evento { get; set; }

        public int idItem { get; set; }
        public virtual Item item { get; set; }

        public decimal quantidade { get; set; }  // Quantidade total de itens calculados para o evento
        
    }
}
