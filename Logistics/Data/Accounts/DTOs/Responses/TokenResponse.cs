using System.ComponentModel.DataAnnotations;

namespace Logistics.Data.Account.AccountDTOs.Responses
{
    public class TokenResponse
    {
        public string accessToken { get; set; }

        public string refreshToken { get; set; }

        public TokenResponse(string accessToken, string refreshToken)
        {
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
        }
    }
}
