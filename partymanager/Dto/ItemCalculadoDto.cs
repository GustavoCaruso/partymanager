namespace partymanager.Application.Dto
{
    public class ItemCalculadoDto
    {
        public int id { get; set; }
        public int idEvento { get; set; }  // Chave estrangeira do Evento
        public int idItem { get; set; }  // Chave estrangeira do Item
        public decimal quantidade { get; set; }
       
    }
}
