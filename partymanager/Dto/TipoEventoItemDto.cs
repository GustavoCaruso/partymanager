namespace partymanager.Application.Dto
{
    public class TipoEventoItemDto
    {
        public int id { get; set; }
        public int idTipoEvento { get; set; }  // Chave estrangeira do TipoEvento
        public int idItem { get; set; }
        public decimal quantidadePorPessoa { get; set; }
        
    }
}
