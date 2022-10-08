using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.AuthServices
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new NetworkCredential("caroonlinehutech@gmail.com", "Caro@123");
            smtpClient.EnableSsl = true;

            smtpClient.Send("caroonlinehutech@gmail.com", email, subject, htmlMessage);
            return Task.Run(() => { Console.WriteLine(htmlMessage); });
        }
    }
}
