using Logistics.Data.Account.Models;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.Models;

namespace Logistics.Data.Transportations.DTOs.Responses
{
    public class ShipperTransportationResponseDTO
    {
        public Guid id { get; set; }

        public Company transporter { get; set; }

        public string transport { get; set; }

        public ShipperTransportationResponseDTO(Request request)
        {
            id = request.transportation.id;
            transporter = request.transportation.transporter.company;

            Truck truck = request.transportation.transporter.truck;
            transport = truck.carBrand + " " + truck.model;
        }
    }
}
