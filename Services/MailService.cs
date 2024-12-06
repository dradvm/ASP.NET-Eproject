using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace ABCDMall.Services
{
    public class MailService
    {
        private static readonly string EMAIL = ConfigurationManager.AppSettings["email"];
        private static readonly string PASSWORD = ConfigurationManager.AppSettings["password"];
        private static readonly SmtpClient SMTP = new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(EMAIL, PASSWORD),
            Timeout = 20000
        };

        public static void SendEmail(string receiver, string subject, string body)
        {
            using (MailMessage mailMessage = new MailMessage(EMAIL, receiver)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                SMTP.Send(mailMessage);
            }
        }
    }
}