using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using static System.Net.WebRequestMethods;

namespace Logistics.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        private string smtpAddress = "smtp.yandex.ru";
        private bool enableSSL = true;

        private string emailFrom;
        private string password;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            emailFrom = _configuration["EmailSettings:Email"];
            password = _configuration["EmailSettings:Password"];
        }

        public async Task SendRequestPasswordRequest(string emailTo, string token)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("noreply@logistics.com", emailFrom));
            emailMessage.To.Add(new MailboxAddress("", emailTo));
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<a href=http://localhost:5169/resetPassword?token=" + token + ">Reset Password</a>"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpAddress, 465, true);
                await client.AuthenticateAsync(emailFrom, password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        public async Task SendApproveEmailRequest(string emailTo, string token)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("noreply@logistics.com", emailFrom));
            emailMessage.To.Add(new MailboxAddress("", emailTo));
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<a href=http://localhost:5169/approveEmail?token=" + token + ">Approve Email</a>"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpAddress, 465, enableSSL);
                await client.AuthenticateAsync(emailFrom, password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
