using Logistics.Data.Account.Models;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.DTOs.Responses;
using Logistics.Data.Requests.Models;
using Logistics.Data.Transportations.Models;

namespace Logistics.Data.Transportations.DTOs.Responses
{

    public class TransportationResponseDTO
    {
        public Guid id { get; set; }
        public TransportationStatus status { get; set; }

        public City loadCity { get; set; }
        public City unloadCity { get; set; }

        public DateTime? sendingTimeFrom { get; set; }
        public DateTime sendingTime { get; set; }
        public DateTime? arrivalTime { get; set; }

        public TransportationResponseDTO(Transportation transportation)
        {
            id = transportation.id;
            status = transportation.status;

            loadCity = transportation.request.loadCity;
            unloadCity = transportation.request.unloadCity;

            sendingTime = transportation.request.sendingTime;
            sendingTimeFrom = transportation.request.sendingTimeFrom;
            arrivalTime = transportation.request.arrivalTime;
        }
    }
    public class TransporterTransportationResponseDTO : TransportationResponseDTO
    {
        public CompanyResponseWithPhone company { get; set; }
        public ShipmentResponse shipment { get; set; }

        public TransporterTransportationResponseDTO(Transportation transportation) : base(transportation)
        {
            company = new CompanyResponseWithPhone(transportation.request.shipper);
            shipment = new ShipmentResponse(transportation.request.shipment);
        }
    }

    public class ShipperTransportationResponseDTO : TransportationResponseDTO
    {
        public CompanyResponseWithPhone transporter { get; set; }
        public string truckBrand { get; set; }
        public string truckModel { get; set; }

        public ShipperTransportationResponseDTO(Transportation transportation) : base(transportation)
        {
            transporter = new CompanyResponseWithPhone(transportation.transporter);
            truckBrand = transportation.transporter.truck.truckBrand.ToString();
            truckBrand = transportation.transporter.truck.model;
        }
    }
}
