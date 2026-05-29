using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MarketList_API.Response;
using MarketList_Business;
using MarketList_Data;
using MarketList_Model;
using Microsoft.AspNetCore.Mvc;

namespace MarketList_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemBusiness _itemBusiness;
        private readonly IMapper _mapper;

        public ItemController(IItemBusiness itemBusiness, IMapper mapper)
        {
            _itemBusiness = itemBusiness;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetUnidade")]
        public async Task<IActionResult> GetItemPorUnidade(int unidadeId)
        {
            try
            {
                var itensDTO = await _itemBusiness.GetItemPorUnidade(unidadeId);
                var itensVM = _mapper.Map<List<ItemVM>>(itensDTO);
                return Ok(itensVM);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _itemBusiness.GetAllAsync();
                return Ok(new ApiResponse(result, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _itemBusiness.FindByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponse("Item não encontrado", HttpStatusCode.NotFound));

                return Ok(new ApiResponse(result, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            if (item == null || !item.IsValid())
                return BadRequest(new ApiResponse("Dados do item inválidos", HttpStatusCode.BadRequest));

            try
            {
                var result = await _itemBusiness.AddAsync(item);
                return Created(string.Empty, new ApiResponse(result, HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Item item)
        {
            if (item == null || !item.IsValid() || id != item.Id)
                return BadRequest(new ApiResponse("Dados do item inválidos", HttpStatusCode.BadRequest));

            try
            {
                var dbItem = await _itemBusiness.FindByIdAsync(id);
                if (dbItem == null)
                    return NotFound(new ApiResponse("Item não encontrado", HttpStatusCode.NotFound));

                dbItem.SNome = item.SNome;
                dbItem.SUnidadeMedida = item.SUnidadeMedida;
                dbItem.NIdSessao = item.NIdSessao;
                dbItem.NIdUnidade = item.NIdUnidade;
                dbItem.NIdSetor = item.NIdSetor;

                await _itemBusiness.UpdateAsync(dbItem);
                return Ok(new ApiResponse(dbItem, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dbItem = await _itemBusiness.FindByIdAsync(id);
                if (dbItem == null)
                    return NotFound(new ApiResponse("Item não encontrado", HttpStatusCode.NotFound));

                await _itemBusiness.RemoveAsync(id);
                return Ok(new ApiResponse("Item removido com sucesso", HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}