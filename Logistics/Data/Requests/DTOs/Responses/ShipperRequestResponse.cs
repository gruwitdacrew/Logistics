using Logistics.Data.Account.Models;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Requests.Models;

namespace Logistics.Data.Requests.DTOs.Responses
{
    public class RequestResponse
    {
        public Guid id { get; set; }

        public ShipmentResponse shipment { get; set; }

        public RequestStatus status { get; set; }

        public string loadAddress { get; set; }

        public City loadCity { get; set; }

        public City unloadCity { get; set; }

        public string unloadAddress { get; set; }

        public DateTime? sendingTimeFrom { get; set; }
        public DateTime sendingTime { get; set; }
        public DateTime? arrivalTime { get; set; }

        public TruckType truckType { get; set; }

        public float costInRubles { get; set; }

        public float additionalCostInRubles { get; set; }

        public RequestResponse(Request request)
        {
            id = request.id;
            shipment = new ShipmentResponse(request.shipment);
            status = request.status;
            loadAddress = request.loadAddress;
            loadCity = request.loadCity;
            unloadAddress = request.unloadAddress;
            unloadCity = request.unloadCity;
            sendingTime = request.sendingTime;
            truckType = request.truckType;
            sendingTimeFrom = request.sendingTimeFrom;
            arrivalTime = request.arrivalTime;
            costInRubles = request.costInRubles;
            additionalCostInRubles = request.additionalCostInRubles;
        }
    }

    public class ShipperRequestResponse : RequestResponse
    {
        public ShipperRequestResponse(Request request) : base(request)
        {

        }
    }
    public class TransporterRequestResponse : RequestResponse
    {
        public Company company { get; set; }

        public TransporterRequestResponse(Request request) : base(request)
        {
            company = request.shipper.company;
        }
    }
}
