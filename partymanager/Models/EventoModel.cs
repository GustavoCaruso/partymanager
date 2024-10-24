namespace partymanager.Application.Models
{
    public class EventoModel
    {
        public int id { get; set; }
        public string nome { get; set; }
        public DateTime data { get; set; }
        public string localidade { get; set; }
        public int numeroAdultos { get; set; }
        public int numeroCriancas { get; set; }
       
        public int idTipoEvento { get; set; }  // Chave estrangeira do TipoEvento
        public int? idUsuario { get; set; }  // Chave estrangeira opcional do Usuario
    }

}
