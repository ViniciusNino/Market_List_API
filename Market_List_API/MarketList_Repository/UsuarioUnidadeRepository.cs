using MarketList_Data;
using MarketList_Model;

namespace MarketList_Repository;
public class UsuarioUnidadeRepository : BaseRepository<UsuarioUnidade>, IUsuarioUnidadeRepository
{
    public UsuarioUnidadeRepository(MarketListContext ctx): base(ctx)
    {
        
    }
}