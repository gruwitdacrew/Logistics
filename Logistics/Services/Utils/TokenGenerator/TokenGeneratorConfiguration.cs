namespace Logistics.Services.Utils.TokenGenerator
{
    public class TokenGeneratorConfiguration
    {
        public string AccessTokenSecret { get; set; }
        public int AccessTokenExpirationSeconds { get; set; }
        public int RefreshTokenExpirationHours { get; set; }
        public int ApproveEmailTokenExpirationMinutes { get; set; }
        public int ResetPasswordTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

    }
}
