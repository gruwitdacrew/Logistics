using Logistics.Data.Account.AccountDTOs.Requests;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.Models;

namespace Logistics.Data.Account.Models
{
    public class Shipper : User
    {

        public Shipper() { }

        public Shipper(RegisterRequestDTO registerRequest) : base(registerRequest)
        {
            role = Role.Shipper;
        }
    }
}
