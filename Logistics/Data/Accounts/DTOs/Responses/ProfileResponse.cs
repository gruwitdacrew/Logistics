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

    public class ShipperProfileResponse : ProfileResponse
    {
        public ShipperProfileResponse(Shipper shipper) : base(shipper)
        {

        }
    }

    public class TransporterProfileResponse : ProfileResponse
    {
        public string permanentResidence { get; set; }
        public TransporterProfileResponse(Transporter transporter) : base(transporter)
        {
            permanentResidence = transporter.permanentResidence;
        }
    }
}
