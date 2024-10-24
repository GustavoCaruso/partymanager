using partymanager.Domain.Entidades;
using System.Text.Json.Serialization;

namespace partymanager.Application.Models
{
    public class ItemModel
    {
        public int id { get; set; }
        public string nome { get; set; }
       
        public int idTipoItem { get; set; }
       
    }
}
