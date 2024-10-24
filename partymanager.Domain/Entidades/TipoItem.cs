using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Domain.Entidades
{
    public class TipoItem : BaseEntity
    {
        public string nome { get; set; }
        public virtual ICollection<Item> item { get; set; } = new List<Item>();
    }
}
