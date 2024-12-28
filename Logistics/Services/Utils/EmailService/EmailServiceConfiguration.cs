namespace Logistics.Services.Utils.EmailService
{
    public class EmailServiceConfiguration
    {
        public string Email {  get; set; } = Environment.GetEnvironmentVariable("MAILING_EMAIL");
        public string Password { get; set; } = Environment.GetEnvironmentVariable("MAILING_EMAIL_PASSWORD");
    }
}
