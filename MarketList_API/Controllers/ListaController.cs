using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MarketList_Business;
using MarketList_Model;
using Microsoft.AspNetCore.Mvc;

namespace MarketList_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListaController : ControllerBase
    {
        private readonly IListaBusiness _listaBusiness;

        private readonly IMapper _mapper;

        public ListaController(IListaBusiness listaBusiness, IMapper mapper)
        {
            _listaBusiness = listaBusiness;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetUnidade")]
        public async Task<IActionResult> GetListaPorUnidadeId(int unidadeId)
        {
            try
            {
                var listas = await _listaBusiness.GetListaPorUnidadeId(unidadeId);
                var listaVM = _mapper.Map<List<ListaVM>>(listas);
                
                return Ok(listaVM);
            }
            catch (Exception ex)
            {
                throw new Exception("[ListaController/GetListaPorUnidadeId] - " + ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("SalvarLista")]
        public async Task<IActionResult> SetLista(SalvarListaDTO lista)
        {
            try
            {
                var retorno = await _listaBusiness.SetLista(lista);

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                throw new Exception("[ListaController/SetLista] - " + ex.Message, ex);
            }
        }

        [HttpDelete]
        [Route("Deletar/{listaId}")]
        public async Task<IActionResult> DeleteLista(int listaId)
        {
            try
            {
                var retorno = await _listaBusiness.DeleteLista(listaId);

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                throw new Exception("[ListaController/SetLista] - " + ex.Message, ex);
            }
        }
    }
}