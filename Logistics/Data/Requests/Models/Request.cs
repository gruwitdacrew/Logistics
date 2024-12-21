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
            if (editRequest.unloadCity != null) unloadCity = (City)editRequest.unloadCity;
            if (editRequest.loadCity != null) loadCity = (City)editRequest.loadCity;
            if (editRequest.unloadAddress != null) unloadAddress = editRequest.unloadAddress;
            if (editRequest.loadAddress != null) loadAddress = editRequest.loadAddress;
            if (editRequest.sendingTime != null) sendingTime = (DateTime)editRequest.sendingTime;
            if (editRequest.sendingTimeFrom != null) sendingTimeFrom = editRequest.sendingTimeFrom;
            if (editRequest.truckType != null) truckType = (TruckType)editRequest.truckType;
            if (editRequest.distanceBetweenCitiesInKilometers != null) costInRubles = CostCalculator.calculateCostInRubles((int)editRequest.distanceBetweenCitiesInKilometers);

            if (editRequest.shipment != null)
            {
                if (editRequest.shipment.type != null) shipment.type = (ShipmentType)editRequest.shipment.type;
                if (editRequest.shipment.lengthInMeters != null) shipment.lengthInMeters = (float)editRequest.shipment.lengthInMeters;
                if (editRequest.shipment.widthInMeters != null) shipment.widthInMeters = (float)editRequest.shipment.widthInMeters;
                if (editRequest.shipment.heightInMeters != null) shipment.heightInMeters = (float)editRequest.shipment.heightInMeters;
                if (editRequest.shipment.weightInTons != null) shipment.weightInTons = (float)editRequest.shipment.weightInTons;
            }
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
