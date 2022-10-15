using System;
using System.Collections.Generic;

namespace MarketList_Model.DTO
{
    public class ListaAtualizarDTO
    {
        public int ListaId { get; set; }
        public int UsuarioLogadoId { get; set; }
        public List<ItemListaAtualizarDTO> ItensLista { get; set; }
    }

    public class ItemListaAtualizarDTO
    {
        public int? Id { get; set; }
        public int? ItemId { get; set; }
        public int? Quantidade { get; set; }
    }
}