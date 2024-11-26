using Logistics.Services;
using System.ComponentModel.DataAnnotations;
using static Logistics.Services.UserService;

namespace Logistics.Data.Account.AccountDTOs.Requests
{
    public class LoginRequestDTO
    {

        public string email { get; set; }

        public string password { get; set; }
    }
}
