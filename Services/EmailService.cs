using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BlogV6.Services
{
    public class EmailService
    {
        private readonly IConfiguration Configuration;

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public bool Send(
            string toName,
            string toEmail,
            string subject,
            string body,
            string fromName = "Wagner Vieira",
            string fromEmail = "wagnervieira1209@gmail.com"
        )
        {
            var smtpClient = new SmtpClient(Configuration["Smtp:Host"], Int32.Parse(Configuration["Smtp:Port"]));
            smtpClient.Credentials = new NetworkCredential(Configuration["Smtp:Username"], Configuration["Smtp:Password"]);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            var mail = new MailMessage();

            mail.From = new MailAddress(fromEmail, fromName);
            mail.To.Add(new MailAddress(toEmail, toName));
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                erro += ex.StackTrace;
                return false;
            }
        }
    }
}