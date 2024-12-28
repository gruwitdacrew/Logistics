using Logistics.Services.Utils.EmailService;
using Logistics.Services.Utils.TokenGenerator;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace Logistics.Services
{
    public class EmailService
    {
        private string smtpAddress = "smtp.yandex.ru";
        private bool enableSSL = true;

        private EmailServiceConfiguration emailServiceConfiguration = new EmailServiceConfiguration();

        public EmailService(IConfiguration configuration)
        {
            configuration.Bind("EmailSettings", emailServiceConfiguration);
        }

        private string getApproveMailContent(string token)
        {
            return @"  
            <!DOCTYPE html>  
            <html lang='ru'>  
            <head>  
                <meta charset='UTF-8'>  
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>  
                <title>Подтверждение почты</title>  
                <style>  
                    body {  
                        font-family: Arial, sans-serif;  
                        margin: 0;  
                        padding: 0;  
                        background-color: #f4f4f4;  
                    }  
                    .container {  
                        width: 100%;  
                        max-width: 600px;  
                        margin: 20px auto;  
                        background: #ffffff;  
                        border-radius: 8px;  
                        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);  
                        padding: 20px;  
                    }  
                    h1 {  
                        color: #333;  
                    }  
                    .button {  
                        display: inline-block;  
                        padding: 10px 20px;  
                        margin-top: 20px;  
                        background-color: #007BFF;  
                        color: white;  
                        text-decoration: none;  
                        border-radius: 5px;  
                        transition: background-color 0.3s;  
                    }  
                    .button:hover {  
                        background-color: #0056b3;  
                    }  
                </style>  
            </head>  
            <body>  
                <div class='container'>  
                    <h1>Подтверждение почты</h1>  
                    <p>Нажмите на кнопку ниже, чтобы подтвердить почту</p>  
                    <a href='http://localhost:5173/email/approve?token=" + token + @"' class='button'>Подтвердить</a>  
                    <p>Если вы не запрашивали сброс пароля, просто проигнорируйте это письмо.</p>  
                </div>  
            </body>  
            </html>";
        }

        private string getResetMailContent(string token)
        {
            return @"  
            <!DOCTYPE html>  
            <html lang='ru'>  
            <head>  
                <meta charset='UTF-8'>  
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>  
                <title>Сброс пароля</title>  
                <style>  
                    body {  
                        font-family: Arial, sans-serif;  
                        margin: 0;  
                        padding: 0;  
                        background-color: #f4f4f4;  
                    }  
                    .container {  
                        width: 100%;  
                        max-width: 600px;  
                        margin: 20px auto;  
                        background: #ffffff;  
                        border-radius: 8px;  
                        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);  
                        padding: 20px;  
                    }  
                    h1 {  
                        color: #333;  
                    }  
                    .button {  
                        display: inline-block;  
                        padding: 10px 20px;  
                        margin-top: 20px;  
                        background-color: #007BFF;  
                        color: white;  
                        text-decoration: none;  
                        border-radius: 5px;  
                        transition: background-color 0.3s;  
                    }  
                    .button:hover {  
                        background-color: #0056b3;  
                    }  
                </style>  
            </head>  
            <body>  
                <div class='container'>  
                    <h1>Сброс пароля</h1>  
                    <p>Вы запрашивали сброс пароля. Нажмите на кнопку ниже, чтобы сбросить его.</p>  
                    <a href='http://localhost:5173/password/reset?token=" + token + @"' class='button'>Сбросить пароль</a>  
                    <p>Если вы не запрашивали сброс пароля, просто проигнорируйте это письмо.</p>  
                </div>  
            </body>  
            </html>";
        }

        public Task SendResetPasswordRequest(string emailTo, string token)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("noreply@logistics.com", emailServiceConfiguration.Email));
            emailMessage.To.Add(new MailboxAddress("", emailTo));
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = getResetMailContent(token)
            };

            using (var client = new SmtpClient())
            {
                client.Connect(smtpAddress, 465, true);
                client.Authenticate(emailServiceConfiguration.Email, emailServiceConfiguration.Password);
                client.Send(emailMessage);

                client.Disconnect(true);
            }
            return Task.CompletedTask;
        }

        public Task SendApproveEmailRequest(string emailTo, string token)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("noreply@logistics.com", emailServiceConfiguration.Email));
            emailMessage.To.Add(new MailboxAddress("", emailTo));
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = getApproveMailContent(token)
            };

            using (var client = new SmtpClient())
            {
                client.Connect(smtpAddress, 465, enableSSL);
                client.Authenticate(emailServiceConfiguration.Email, emailServiceConfiguration.Password);
                client.Send(emailMessage);

                client.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
