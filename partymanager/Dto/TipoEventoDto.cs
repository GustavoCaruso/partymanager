using partymanager.Domain.Entidades;

namespace partymanager.Application.Dto
{
    public class TipoEventoDto
    {
        public int id { get; set; }
        public string nome { get; set; }

        // Lista de itens associados ao tipo de evento
        public List<TipoEventoItemDto> Itens { get; set; } = new List<TipoEventoItemDto>();

    }
}
