using Logistics.Data.Account.Models;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.DTOs.Responses;
using Logistics.Data.Requests.Models;
using Logistics.Data.Transportations.Models;

namespace Logistics.Data.Transportations.DTOs.Responses
{

    public class TransportationWideResponseDTO
    {
        public Guid id { get; set; }

        public TransportationStatus status { get; set; }

        public City loadCity { get; set; }
        public DateTime loadDate { get; set; }
        public string loadAddress { get; set; }

        public City unloadCity { get; set; }
        public DateTime unloadDate { get; set; }
        public string unloadAddress { get; set; }

        public DateTime? sendingTimeFrom { get; set; }
        public DateTime sendingTime { get; set; }
        public DateTime? arrivalTime { get; set; }

        public List<TransportationStatusChangeResponseDTO> statusChangeHistory { get; set; }

        public TransportationWideResponseDTO(Transportation transportation, List<TransportationStatusChangeResponseDTO> statusChangeHistory)
        {
            id = transportation.id;

            status = transportation.status;

            loadCity = transportation.request.loadCity;
            loadDate = statusChangeHistory.Where(x => x.status == TransportationStatus.Loading).Select(x => x.time).First();
            loadAddress = transportation.request.loadAddress;

            unloadCity = transportation.request.unloadCity;
            unloadDate = statusChangeHistory.Where(x => x.status == TransportationStatus.Unloading).Select(x => x.time).First();
            unloadAddress = transportation.request.unloadAddress;

            sendingTime = transportation.request.sendingTime;
            sendingTimeFrom = transportation.request.sendingTimeFrom;

            arrivalTime = transportation.request.arrivalTime;

            this.statusChangeHistory = statusChangeHistory;
        }
    }

    public class TransporterTransportationWideResponseDTO : TransportationWideResponseDTO
    {
        public Company company { get; set; }
        public ShipmentResponse shipment { get; set; }

        public TransporterTransportationWideResponseDTO(Transportation transportation, List<TransportationStatusChangeResponseDTO> statusChangeHistory) : base(transportation, statusChangeHistory)
        {
            company = transportation.request.shipper.company;
            shipment = new ShipmentResponse(transportation.request.shipment);
        }
    }

    public class ShipperTransportationWideResponseDTO : TransportationWideResponseDTO
    {
        public Company transporter { get; set; }
        public string truckBrand { get; set; }
        public string truckModel { get; set; }

        public ShipperTransportationWideResponseDTO(Transportation transportation, List<TransportationStatusChangeResponseDTO> statusChangeHistory) : base(transportation, statusChangeHistory)
        {
            transporter = transportation.transporter.company;
            truckBrand = transportation.transporter.truck.truckBrand.ToString();
            truckBrand = transportation.transporter.truck.model;
        }
    }
}
