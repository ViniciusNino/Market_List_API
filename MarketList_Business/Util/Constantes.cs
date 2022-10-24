using System.Text;
using MarketList_API.Data;
using MarketList_Model;

namespace MarketList_API.Util
{
    public static class Constantes
    {
        public static string TituloEmail_AtivarEmail = "Ativação de Email";
        public static string TituloEmail_SenhaTemporaria = "Senha Temporária";

        public static string Getsubject(int tipo)
        {
            return tipo == (int)TipoVerificacaoTokenEnum.Ativacao_Email ? TituloEmail_AtivarEmail : TituloEmail_SenhaTemporaria;
        }

        public static string GetBody(int tipo, string token)
        {
            var apiConnection = Common.GetApplicationUrl();
            var emailMarktList = Common.GetMailSettings("Mail");
            var rota = Common.GetMailSettings("ConfirmEmailPath");
            var password = Common.GetMailSettings("Password");
            var body = new StringBuilder();

            if (tipo == (int)TipoVerificacaoTokenEnum.Ativacao_Email)
            {
                body.Append("<h1>Bem vindo ao app da Market List!<h1>");
                body.Append("<p>Parece que você solicitou a criação de uma conta. Se você não iniciou esta solicitação ou não deseja criar uma conta, ignore este e-mail. Este link expira em 10 minutos*</p>");
                body.Append($"<p>Por favor, clique no link para confirmar seu e-mail, <a href='{apiConnection}{rota}{token}'>Confirmar Email</a></p>");
                body.Append("<p>Obrigado</p>");
                body.Append("<p>Time Market List</p>");
            }
            else
            {
                body.Append("<h1>Olá!<h1>");
                body.Append("<p>Parece que você solicitou uma senha temporária. Se você não iniciou esta solicitação, ignore este e-mail. Este link expira em 1 hora*</p>");
                body.Append("<p>Altere sua senha após logar, pois está expirará em 1 hora</p>");
                body.Append("<p>Senha temporária</p>");
                body.Append($"<p>{token}</p>");
                body.Append("<p>Obrigado</p>");
                body.Append("<p>Time Market List</p>");
            }

            return body.ToString();
        }
    }
}