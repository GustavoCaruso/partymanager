using partymanager.Domain.Entidades;

namespace partymanager.Application.Models
{
    public class TipoEventoItemModel
    {
        public int id { get; set; }
        public int idTipoEvento { get; set; }
        public int idItem { get; set; }
        public decimal quantidadePorPessoa { get; set; }  // Define a quantidade de itens por pessoa
        
    }

}
