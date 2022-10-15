using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Model.DTO;

namespace MarketList_Business
{
    public interface IItemListaBusiness : IBaseBusiness<ItemLista>
    {
        Task<List<ItemListaDTO>> GetGetItensListaPorListaId(int listaId);
        Task<int> AtualizarItensLista(ListaAtualizarDTO listaAtualizarDto);
    }
}