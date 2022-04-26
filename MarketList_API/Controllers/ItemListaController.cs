using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MarketList_Business;
using MarketList_Model;
using MarketList_Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MarketList_API.Controllers
{   
    [ApiController]
    [Route("[controller]")]
    public class ItemListaController : ControllerBase
    {
        private readonly IItemListaBusiness _itemListaBussiness;
        private readonly IMapper _mapper;

        public ItemListaController(IItemListaBusiness itemListaBussiness, IMapper mapper)
        {
            _itemListaBussiness = itemListaBussiness;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetLista")]
        public async Task<IActionResult> GetItensListaPorListaId(int nIdLista)
        {
            try
            {
                var itensListaDTO = await _itemListaBussiness.GetGetItensListaPorListaId(nIdLista);
                var itensListaVM = _mapper.Map<List<ItemListaVM>>(itensListaDTO);
                return Ok(itensListaVM);
            }
            catch (Exception ex)
            {
                throw new Exception($"[ItemListaController - GetItensListaPorListaId] - {ex.Message}", ex);
            }
        }   

        
        [HttpPost]
        [Route("Atualizar")]
        public async Task<IActionResult> UpdateLista(ListaAtualizarDTO listaAtualizarDto)
        {
            try
            {
                var response = await _itemListaBussiness.AtualizarItensLista(listaAtualizarDto);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"[ItemListaController - UpdateLista] - {ex.Message}", ex);
            }
        }    
    }
}