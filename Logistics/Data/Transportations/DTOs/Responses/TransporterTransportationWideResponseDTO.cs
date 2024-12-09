using Logistics.Data.Account.Models;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.DTOs.Responses;
using Logistics.Data.Requests.Models;
using Logistics.Data.Transportations.Models;

namespace Logistics.Data.Transportations.DTOs.Responses
{
    public class TransporterTransportationWideResponseDTO
    {
        public Guid id { get; set; }

        public Company company { get; set; }

        public ShipmentResponse shipment { get; set; }

        public TransportationStatus status { get; set; }

        public string loadCity { get; set; }
        public string loadAddress { get; set; }

        public string unloadCity { get; set; }
        public string unloadAddress { get; set; }

        public string? sendingTimeFrom { get; set; }
        public string sendingTime { get; set; }

        public List<TransportationStatusChangeResponseDTO> statusChangeHistory { get; set; }

        public TransporterTransportationWideResponseDTO(Transportation transportation, List<TransportationStatusChangeResponseDTO> statusChangeHistory)
        {
            id = transportation.id;
            company = transportation.request.shipper.company;
            shipment = new ShipmentResponse(transportation.request.shipment);

            status = transportation.status;

            loadCity = transportation.request.loadCity;
            loadAddress = transportation.request.loadAddress;
            unloadCity = transportation.request.unloadCity;
            unloadAddress = transportation.request.unloadAddress;
            sendingTime = transportation.request.sendingTime.ToString("dd MMMM yyyy, HH:mm", new System.Globalization.CultureInfo("ru-RU"));
            sendingTimeFrom = ((DateTime)transportation.request.sendingTimeFrom).ToString("dd MMMM yyyy, HH:mm", new System.Globalization.CultureInfo("ru-RU"));
            this.statusChangeHistory = statusChangeHistory;
        }
    }
}
