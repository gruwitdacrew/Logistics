using Logistics.Data.Account.Models;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.Models;

namespace Logistics.Data.Requests.DTOs.Responses
{
    public class TransporterRequestResponse : ShipperRequestResponse
    {
        public Company company { get; set; }

        public TransporterRequestResponse(Request request) : base(request)
        {
            company = request.shipper.company;
        }
    }
}
