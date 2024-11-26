using Logistics.Data.Account.Models;
using Logistics.Data.Requests.Models;

namespace Logistics.Data.Requests.RequestDTOs.Responses
{
    public class RequestResponse
    {
        public Guid id { get; set; }

        public ShipmentResponse shipment { get; set; }

        public RequestStatus status { get; set; }

        public string loadAddress { get; set; }

        public string unloadAddress { get; set; }

        public DateTime sendingTime { get; set; }

        public TruckType truckType { get; set; }

        public DateTime? desiredDeliveryTime { get; set; }

        public float costInRubles { get; set; }

        public RequestResponse(Request request)
        {
            id = request.id;
            shipment = new ShipmentResponse(request.shipment);
            status = request.status;
            loadAddress = request.loadAddress;
            unloadAddress = request.unloadAddress;
            sendingTime = request.sendingTime;
            truckType = request.truckType;
            desiredDeliveryTime = request.desiredDeliveryTime;
            costInRubles = request.costInRubles;
        }
    }
}
