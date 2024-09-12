using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;

namespace SyncChat.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }   

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailObj = new MimeMessage();

            emailObj.From.Add(new MailboxAddress("SyncChat", _configuration.GetSection("MailerSend:SenderEmail").Value));
            emailObj.To.Add(new MailboxAddress("Receiver", email));

            emailObj.Subject = subject;
            emailObj.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.mailersend.net", 587, false);
            await smtp.AuthenticateAsync(_configuration.GetSection("MailerSend:SenderEmail").Value, _configuration.GetSection("MailerSend:SenderPassword").Value);
            await smtp.SendAsync(emailObj);
            await smtp.DisconnectAsync(true);
        }
    }
}
