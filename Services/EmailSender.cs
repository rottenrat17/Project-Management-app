using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace Comp2139Lab1.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_emailSettings.MailServer, _emailSettings.MailPort)
            {
                Credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password),
                EnableSsl = true
            };
            
            return client.SendMailAsync(
                new MailMessage(_emailSettings.Sender, email, subject, htmlMessage) { IsBodyHtml = true }
            );
        }
    }

    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
    }
} 