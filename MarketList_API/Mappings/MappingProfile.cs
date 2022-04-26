using System.Collections.Generic;
using AutoMapper;
using MarketList_Model;

namespace MarketList_API
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            CreateMap<ItemDTO, ItemVM>();
            CreateMap<ListaDTO, ListaVM>(); 
            CreateMap<ItemListaDTO, ItemListaVM>();
        }
    }
}