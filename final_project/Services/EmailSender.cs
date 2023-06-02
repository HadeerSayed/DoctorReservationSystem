using Azure.Core;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using System.Net.Mail;

namespace final_project.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var mail = "ayya7098@outlook.com";
            var pw = "Aya2010@1998";

            var message = new MailMessage();
            message.From = new MailAddress(mail);
            message.Subject = subject;
            message.To.Add(email);
            message.Body = $"<html><body>{htmlMessage}</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(mail, pw),
                EnableSsl = true
            };

            smtpClient.Send(message);

        }
    }
}
