using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Model.DTO;

namespace MarketList_Business
{
    public interface IVerificacaoTokenBusiness : IBaseBusiness<VerificacaoToken>
    {

        Task<bool> SetVerificacaoToken(UsuarioCadastroDTO user, int TipoVerificacao);
        bool TokenValido(VerificacaoToken tokenDB);
    }
}