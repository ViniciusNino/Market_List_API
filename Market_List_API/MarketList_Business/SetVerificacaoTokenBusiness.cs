using System.Threading.Tasks;
using MarketList_API.Util;
using MarketList_Model;
using MarketList_Model.DTO;
using MarketList_Repository;

namespace MarketList_Business
{
    public class SetVerificacaoTokenBusiness : BaseBusiness<VerificacaoToken>, ISetVerificacaoTokenBusiness
    {
        public SetVerificacaoTokenBusiness(IVerificacaoTokenRepository verificacaoTokenRepository, IUnidadeDeTrabalho unidadeTrab) : base(verificacaoTokenRepository, unidadeTrab)
        { }

        public async Task<bool> SetVerificacaoToken(UsuarioCadastroDTO user, int tipoVerificacao)
        {
            var token = new VerificacaoToken(user.Id, tipoVerificacao);
            var tokenEmail = token.GetTokenEmal();

            await AddAsync(token);

            SendEmail.Send(user.Email, tokenEmail, tipoVerificacao);


            return true;
        }
    }
}