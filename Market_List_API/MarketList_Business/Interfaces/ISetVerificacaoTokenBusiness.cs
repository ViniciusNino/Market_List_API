using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Model.DTO;

namespace MarketList_Business
{
    public interface ISetVerificacaoTokenBusiness : IBaseBusiness<VerificacaoToken>
    {
        Task<bool> SetVerificacaoToken(UsuarioCadastroDTO user, int TipoVerificacao);
    }
}