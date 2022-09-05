using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Business
{
    public interface IUnidadeBusiness : IBaseBusiness<Unidade>
    {
        Task<List<UnidadeVM>> GetUnidadesEListas(int usuarioId);
    }
}