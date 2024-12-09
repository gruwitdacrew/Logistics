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

        public TransporterTransportationResponseDTO(Transportation transportation)
        {
            id = transportation.id;
            status = transportation.status;
            company = transportation.request.shipper.company;
            shipment = new ShipmentResponse(transportation.request.shipment);
        }
    }
}
