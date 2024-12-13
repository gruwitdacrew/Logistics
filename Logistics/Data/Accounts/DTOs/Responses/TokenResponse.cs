using Logistics.Data.Account.Models;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Data.Account.AccountDTOs.Responses
{
    public class TokenResponse
    {
        public Role role { get; set; }

        public string accessToken { get; set; }

        public string refreshToken { get; set; }

        public TokenResponse(User user, string accessToken, string refreshToken)
        {
            this.role = user.role;
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
        }
    }
}
