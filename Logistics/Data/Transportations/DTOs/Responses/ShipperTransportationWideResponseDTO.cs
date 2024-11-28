using Logistics.Data.Account.Models;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.DTOs.Responses;
using Logistics.Data.Requests.Models;

namespace Logistics.Data.Transportations.DTOs.Responses
{
    public class ShipperTransportationWideResponseDTO
    {
        public Guid id { get; set; }

        public Company transporter { get; set; }

        public string transport { get; set; }

        public string loadCity { get; set; }
        public string loadAddress { get; set; }

        public string unloadCity { get; set; }
        public string unloadAddress { get; set; }

        public string sendingTime { get; set; }
        public string desiredDeliveryTime { get; set; }

        public List<TransportationStatusChangeResponseDTO> statusChangeHistory { get; set; }

        public ShipperTransportationWideResponseDTO(Request request, List<TransportationStatusChangeResponseDTO> statusChangeHistory)
        {
            id = request.transportation.id;
            transporter = request.transportation.transporter.company;

            Truck truck = request.transportation.transporter.truck;
            transport = truck.carBrand + " " + truck.model;

            loadCity = request.loadCity;
            loadAddress = request.loadAddress;
            unloadCity = request.unloadCity;
            unloadAddress = request.unloadAddress;

            sendingTime = request.sendingTime.ToString("dd MMMM yyyy, HH:mm", new System.Globalization.CultureInfo("ru-RU"));
            desiredDeliveryTime = ((DateTime)request.desiredDeliveryTime).ToString("dd MMMM yyyy, HH:mm", new System.Globalization.CultureInfo("ru-RU"));

            this.statusChangeHistory = statusChangeHistory;
        }
    }
}
