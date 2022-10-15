using System.Net;
using System;
using System.Net.Mail;
using MarketList_API.Data;
using Microsoft.Extensions.Configuration;

namespace MarketList_API.Util
{
    public static class SendEmail
    {
        public static void Send(string email, string token)
        {
            MailMessage emailMessage = new MailMessage();
            var apiConnection = Common.GetApplicationUrl();
            var emailMarktList = Common.GetMailSettings("Mail");
            var rota = Common.GetMailSettings("ConfirmEmailPath");
            var password = Common.GetMailSettings("Password");
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 60 * 60;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailMarktList, password);

                emailMessage.From = new MailAddress(emailMarktList, "Market List");
                emailMessage.Subject = Constantes.TituloEmail;
                emailMessage.Body = String.Format(Constantes.GetBody().ToString(), apiConnection, rota, token);
                emailMessage.IsBodyHtml = true;
                emailMessage.Priority = MailPriority.Normal;
                emailMessage.To.Add(email);

                smtpClient.Send(emailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"[SendEmail - Send] - {ex.Message}", ex);
            }
        }
    }
}