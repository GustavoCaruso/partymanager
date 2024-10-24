using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Domain.Entidades
{
    public class TipoEvento : BaseEntity
    {
        public string nome { get; set; }

       
        public virtual ICollection<TipoEventoItem> tipoeventoitem { get; set; } = new List<TipoEventoItem>();

        
        public virtual ICollection<Evento> evento { get; set; } = new List<Evento>();
    }
}
