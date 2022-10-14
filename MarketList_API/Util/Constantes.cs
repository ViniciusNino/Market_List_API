using System.Text;

namespace MarketList_API.Util
{
    public static class Constantes
    {
        public static string TituloEmail = "Ativação de Email";

        public static StringBuilder GetBody()
        {
            var body = new StringBuilder();

            body.Append("<h1>Bem vindo ao app da Market List!<h1>");
            body.Append("<p>Parece que você solicitou a criação de uma conta. Se você não iniciou esta solicitação ou não deseja criar uma conta, ignore este e-mail. Este link expira em 10 minutos*</p>");
            body.Append("<p>Por favor, clique no link para confirmar seu e-mail, <a href='{0}{1}{2}'>Confirmar Email</a></p>");
            body.Append("<p>Obrigado</p>");
            body.Append("<p>Time Market List</p>");

            return body;
        }
    }
}