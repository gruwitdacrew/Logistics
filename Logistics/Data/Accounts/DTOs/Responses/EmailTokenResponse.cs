using System.ComponentModel.DataAnnotations;

namespace Logistics.Data.Account.AccountDTOs.Responses
{
    public class EmailTokenResponse
    {
        public string token { get; set; }

        public EmailTokenResponse(string token)
        {
            this.token = token;
        }
    }
}
