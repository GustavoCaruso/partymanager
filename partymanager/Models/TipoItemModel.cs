using partymanager.Domain.Entidades;
using System.Text.Json.Serialization;

namespace partymanager.Application.Models
{
    public class TipoItemModel
    {
        public int id { get; set; }
        public string nome { get; set; }
    }
}
