using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly MarketListContext _ctx;

        private readonly IMapper _mapper;

        public ItemController(IItemBusiness itemBusiness, IMapper mapper, MarketListContext ctx)
        {
            _itemBusiness = itemBusiness;
            _mapper = mapper;
            _ctx = ctx;
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
    }
}