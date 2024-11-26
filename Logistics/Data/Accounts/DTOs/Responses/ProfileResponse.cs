using Logistics.Data.Account.Models;

namespace Logistics.Data.Account.AccountDTOs.Responses
{
    public class ProfileResponse
    {
        public string fullName { get; set; }

        public string email { get; set; }

        public string phone { get; set; }


        public ProfileResponse(User user)
        {
            fullName = user.fullName;
            email = user.email;
            phone = user.phone;
        }
    }
}
