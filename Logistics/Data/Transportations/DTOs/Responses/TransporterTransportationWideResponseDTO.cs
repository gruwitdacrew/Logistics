using Logistics.Data.Account.Models;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.DTOs.Responses;
using Logistics.Data.Requests.Models;

namespace Logistics.Data.Transportations.DTOs.Responses
{
    public class TransporterTransportationWideResponseDTO
    {
        public Guid id { get; set; }

        public Company company { get; set; }

        public ShipmentResponse shipment { get; set; }

        public string loadCity { get; set; }
        public string loadAddress { get; set; }

        public string unloadCity { get; set; }
        public string unloadAddress { get; set; }

        public string? sendingTimeFrom { get; set; }
        public string sendingTime { get; set; }

        public List<TransportationStatusChangeResponseDTO> statusChangeHistory { get; set; }

        public TransporterTransportationWideResponseDTO(Request request, List<TransportationStatusChangeResponseDTO> statusChangeHistory)
        {
            id = request.transportation.id;
            company = request.shipper.company;
            shipment = new ShipmentResponse(request.shipment);
            loadCity = request.loadCity;
            loadAddress = request.loadAddress;
            unloadCity = request.unloadCity;
            unloadAddress = request.unloadAddress;
            sendingTime = request.sendingTime.ToString("dd MMMM yyyy, HH:mm", new System.Globalization.CultureInfo("ru-RU"));
            sendingTimeFrom = ((DateTime)request.sendingTimeFrom).ToString("dd MMMM yyyy, HH:mm", new System.Globalization.CultureInfo("ru-RU"));
            this.statusChangeHistory = statusChangeHistory;
        }
    }
}
