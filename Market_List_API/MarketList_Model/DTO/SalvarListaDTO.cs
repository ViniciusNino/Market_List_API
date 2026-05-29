using System.Collections.Generic;

namespace MarketList_Model
{
    public class SalvarListaDTO : ListaDTO
    {
        public int UsuarioId { get; set; }
        
        public int UnidadeId { get; set; }
        
        public List<ItemListaDTO> ItensLista { get; set; }
    }
}