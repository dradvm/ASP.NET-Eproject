using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace ABCDMall.Services
{
    public class MailService
    {
        private static readonly string EMAIL = "plateportal@gmail.com";
        private static readonly string PASSWORD = "ppkedxfqgtlbgpie";
        private static readonly SmtpClient smtpClient = new SmtpClient()
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
                smtpClient.Send(mailMessage);
            }
        }
    }
}