using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.DTOs.Responses;
using Logistics.Data.Requests.Models;
using Logistics.Data.Transportations.Models;

namespace Logistics.Data.Transportations.DTOs.Responses
{
    public class TransporterTransportationResponseDTO
    {
        public Guid id { get; set; }

        public TransportationStatus status { get; set; }
        public Company company { get; set; }

        public ShipmentResponse shipment { get; set; }

        public TransporterTransportationResponseDTO(Request request)
        {
            id = request.transportation.id;
            status = request.transportation.status;
            company = request.shipper.company;
            shipment = new ShipmentResponse(request.shipment);
        }
    }
}
