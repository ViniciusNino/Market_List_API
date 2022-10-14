using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Repository
{
    public interface IUnidadeRepository : IBaseRepository<Unidade>
    {
        Task<List<UnidadeDTO>> GetUnidadesEListas(int usuarioId);
    }
}