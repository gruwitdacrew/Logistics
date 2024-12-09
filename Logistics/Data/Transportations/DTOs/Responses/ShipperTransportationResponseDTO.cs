using Logistics.Data.Account.Models;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.Models;
using Logistics.Data.Transportations.Models;

namespace Logistics.Data.Transportations.DTOs.Responses
{
    public class ShipperTransportationResponseDTO
    {
        public Guid id { get; set; }

        public TransportationStatus status { get; set; }

        public Company transporter { get; set; }

        public string transport { get; set; }

        public ShipperTransportationResponseDTO(Transportation transportation)
        {
            id = transportation.id;
            transporter = transportation.transporter.company;

            status = transportation.status;

            Truck truck = transportation.transporter.truck;
            transport = truck.truckBrand + " " + truck.model;
        }
    }
}
