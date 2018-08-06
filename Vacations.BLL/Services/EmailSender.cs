using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Vacations.BLL.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(
            IOptions<AuthMessageSenderOptions> optionsAccessor, 
            IConfiguration configuration
            )
        {
            _configuration = configuration;
            Options = optionsAccessor.Value;
            Options.SendGridKey = _configuration["EmailService:SendGridKey"];
            Options.SendGridUser = _configuration["EmailService:SendGridUser"];
        }

        public AuthMessageSenderOptions Options { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_configuration["EmailService:EmailFrom"], _configuration["EmailService:MassageName"]),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }

        public class AuthMessageSenderOptions
        {
            public string SendGridUser { get; set; }
            public string SendGridKey { get; set; }
        }
    }
}