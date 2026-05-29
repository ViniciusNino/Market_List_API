using System.Net;
using System;
using System.Net.Mail;
using MarketList_API.Data;
using MarketList_Model;
using BC = BCrypt.Net.BCrypt;

namespace MarketList_API.Util
{
    public static class SendEmail
    {
        public static void Send(string email, string token, int tipo)
        {
            MailMessage emailMessage = new MailMessage();
            var emailMarktList = Common.GetMailSettings("Mail");
            var password = Common.GetMailSettings("Password");

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 60 * 120;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailMarktList, password);

                emailMessage.From = new MailAddress(emailMarktList, "Market List");
                emailMessage.Subject = Constantes.Getsubject(tipo);
                emailMessage.Body = Constantes.GetBody(tipo, token); ;
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