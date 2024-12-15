using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ABCDMall.Service
{
    public class EmailSender
    {
        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                // Lấy thông tin từ web.config
                string fromEmail = ConfigurationManager.AppSettings["email"];
                string password = ConfigurationManager.AppSettings["password"];

                // Tạo đối tượng MailMessage
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);  // Địa chỉ người nhận
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true; // Nếu muốn gửi email dưới dạng HTML

                // Cấu hình SmtpClient để gửi email qua Gmail
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true; // Bật SSL

                // Gửi email
                smtp.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}