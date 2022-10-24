using System;
using System.Text;
using System.Threading.Tasks;
using MarketList_API.Util;
using MarketList_Model;
using MarketList_Model.DTO;
using MarketList_Repository;
using BC = BCrypt.Net.BCrypt;

namespace MarketList_Business
{
    public class VerificacaoTokenBusiness : BaseBusiness<VerificacaoToken>, IVerificacaoTokenBusiness
    {
        private const int MinutosConfirmarEmail = 10;
        private const int QuantidadeCaracter = 10;
        private readonly IVerificacaoTokenRepository _verificacaoTokenRepository;
        private readonly IUnidadeDeTrabalho _unidadeTrab;

        public VerificacaoTokenBusiness(IVerificacaoTokenRepository verificacaoTokenRepository, IUnidadeDeTrabalho unidadeTrab) : base(verificacaoTokenRepository, unidadeTrab)
        {
            _verificacaoTokenRepository = verificacaoTokenRepository;
            _unidadeTrab = unidadeTrab;
        }

        public async Task<bool> SetVerificacaoToken(UsuarioCadastroDTO user, int tipoVerificacao)
        {
            try
            {
                var tipoEmail = tipoVerificacao == (int)TipoVerificacaoTokenEnum.Ativacao_Email;
                var texto = Token.GetSenhaTemporaria(QuantidadeCaracter);

                var tokenVerificacao = tipoEmail ? CriarTokenEmail() : CriarTokenSenha(texto);

                var verificacaoToken = new VerificacaoToken(user.Id, tipoVerificacao, tokenVerificacao);

                await AddAsync(verificacaoToken);

                var tokenEmail = tipoEmail ? tokenVerificacao : texto;

                SendEmail.Send(user.Email, tokenEmail, tipoVerificacao);


                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"[VerificacaoTokenBusiness - SetVerificacaoToken] - {ex.Message}", ex);
            }
        }

        private string CriarTokenEmail()
        {
            var texto = Guid.NewGuid().ToString();
            var bytesTextoToken = Encoding.UTF8.GetBytes(texto);

            return Convert.ToBase64String(bytesTextoToken);
        }

        private string CriarTokenSenha(string texto)
        {
            return BC.HashPassword(texto);
        }

        public bool TokenValido(VerificacaoToken tokenDB)
        {
            var dataAtual = DateTime.Now;
            var tempoEnvioEmail = (dataAtual - tokenDB.DCadastro).TotalMinutes;

            return tempoEnvioEmail <= MinutosConfirmarEmail;
        }
    }
}