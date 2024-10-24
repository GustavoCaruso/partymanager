using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Domain.Entidades
{
    public abstract class BaseEntity
    {
        public virtual int id { get; set; }
    }
}
