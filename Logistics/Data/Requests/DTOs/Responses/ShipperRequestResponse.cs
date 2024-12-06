using Logistics.Data.Account.Models;
using Logistics.Data.Requests.Models;
using Logistics.Services.Utils;

namespace Logistics.Data.Requests.DTOs.Responses
{
    public class ShipperRequestResponse
    {
        public Guid id { get; set; }

        public ShipmentResponse shipment { get; set; }

        public RequestStatus status { get; set; }

        public string loadAddress { get; set; }

        public string loadCity { get; set; }

        public string unloadCity { get; set; }

        public string unloadAddress { get; set; }

        public string? sendingTimeFrom { get; set; }
        public string sendingTime { get; set; }

        public TruckType truckType { get; set; }

        public float costInRubles { get; set; }

        public float additionalCostInRubles { get; set; }

        public ShipperRequestResponse(Request request)
        {
            id = request.id;
            shipment = new ShipmentResponse(request.shipment);
            status = request.status;
            loadAddress = request.loadAddress;
            loadCity = request.loadCity;
            unloadAddress = request.unloadAddress;
            unloadCity = request.unloadCity;
            sendingTime = request.sendingTime.ToString("dd MMMM yyyy, HH:mm", new System.Globalization.CultureInfo("ru-RU"));
            truckType = request.truckType;
            sendingTimeFrom = ((DateTime)request.sendingTimeFrom).ToString("dd MMMM yyyy, HH:mm", new System.Globalization.CultureInfo("ru-RU"));
            costInRubles = request.costInRubles;
            additionalCostInRubles = request.additionalCostInRubles;
        }
    }
}
