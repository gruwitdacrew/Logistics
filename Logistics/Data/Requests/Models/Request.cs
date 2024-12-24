using Logistics.Data.Account.Models;
using Logistics.Data.Requests.DTOs.Requests;
using Logistics.Data.Transportations.Models;
using Logistics.Services.Utils;

namespace Logistics.Data.Requests.Models
{
    public class Request
    {
        public Guid id { get; set; }

        public Guid shipperId { get; set; }

        public Shipper shipper { get; set; }

        public Shipment shipment { get; set; }

        public Transportation? transportation { get; set; }

        public DateTime creationTime { get; set; }

        public RequestStatus status { get; set; }

        public City loadCity { get; set; }
        public string loadAddress { get; set; }

        public City unloadCity { get; set; }
        public string unloadAddress { get; set; }

        public string? receiverFullName { get; set; }

        public string? receiverContacts { get; set; }


        public DateTime? sendingTimeFrom { get; set; }
        public DateTime sendingTime { get; set; }
        public DateTime? arrivalTime { get; set; }

        public TruckType truckType { get; set; }

        public float costInRubles { get; set; }

        public float additionalCostInRubles { get; set; }

        public Request() { }

        public Request(CreateRequestRequestDTO createRequest, Shipper shipper, bool isDelayed)
        {
            id = Guid.NewGuid();
            this.shipper = shipper;
            shipment = new Shipment(createRequest.shipment, id);
            creationTime = DateTime.UtcNow;
            status = isDelayed ? RequestStatus.Delayed : RequestStatus.Active;
            loadCity = createRequest.loadCity;
            loadAddress = createRequest.loadAddress;
            unloadCity = createRequest.unloadCity;
            unloadAddress = createRequest.unloadAddress;
            sendingTime = createRequest.sendingTime;
            truckType = createRequest.truckType;
            sendingTimeFrom = createRequest.sendingTimeFrom;
            costInRubles = CostCalculator.calculateCostInRubles(createRequest.distanceBetweenCitiesInKilometers);
            additionalCostInRubles = 0;
        }

        public void edit(EditRequestRequestDTO editRequest)
        {
            unloadCity = editRequest.unloadCity;
            loadCity = editRequest.loadCity;
            unloadAddress = editRequest.unloadAddress;
            loadAddress = editRequest.loadAddress;
            sendingTime = editRequest.sendingTime;
            sendingTimeFrom = editRequest.sendingTimeFrom;
            truckType = editRequest.truckType;
            costInRubles = CostCalculator.calculateCostInRubles(editRequest.distanceBetweenCitiesInKilometers);

            shipment.type = editRequest.shipment.type;
            shipment.lengthInMeters = editRequest.shipment.lengthInMeters;
            shipment.widthInMeters = editRequest.shipment.widthInMeters;
            shipment.heightInMeters = editRequest.shipment.heightInMeters;
            shipment.weightInTons = editRequest.shipment.weightInTons;
        }
    }
    public enum RequestStatus
    {
        Active,
        Delayed,
        Accepted,
        Rejected,
        ArchivedNotAccepted,
        ArchivedTransportationFinished
    }

    public enum City
    {
        Tomsk,
        Novosibirsk,
        Kemerovo
    }
}
