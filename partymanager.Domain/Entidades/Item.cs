using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Domain.Entidades
{
    public class Item : BaseEntity
    {
        public string nome { get; set; }
        
        public int idTipoItem { get; set; }
        public virtual TipoItem tipoitem { get; set; }

        public virtual ICollection<TipoEventoItem> tipoeventoitem { get; set; } = new List<TipoEventoItem>();

        public virtual ICollection<ItemCalculado> itemcalculado { get; set; } = new List<ItemCalculado>();
    }
}
