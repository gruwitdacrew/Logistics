namespace Logistics.Services.Utils.TokenGenerator
{
    public class TokenGeneratorConfiguration
    {
        public string AccessTokenSecret { get; set; } = Environment.GetEnvironmentVariable("ACCESS_TOKEN_SECRET");
        public int AccessTokenExpirationSeconds { get; set; } = 600;
        public int RefreshTokenExpirationHours { get; set; } = 24;
        public int ApproveEmailTokenExpirationMinutes { get; set; } = 15;
        public int ResetPasswordTokenExpirationMinutes { get; set; } = 15;
        public string Issuer { get; set; } = "Logistics";
        public string Audience { get; set; } = "Logistics/api";

    }
}
