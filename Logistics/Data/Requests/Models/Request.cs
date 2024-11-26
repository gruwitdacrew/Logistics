using Logistics.Data.Account.Models;
using Logistics.Data.Requests.RequestDTOs.Requests;
using Logistics.Data.Transportations.Models;
using Geocoding;
using Geocoding.Google;
using Logistics.Services.Utils;

namespace Logistics.Data.Requests.Models
{
    public class Request
    {
        public Guid id { get; set; }

        public Shipper shipper { get; set; }

        public Shipment shipment { get; set; }

        public Transportation? transportation { get; set; }

        public DateTime creationTime { get; set; }

        public RequestStatus status { get; set; }

        public string loadCity { get; set; }
        public string loadAddress { get; set; }

        public string unloadCity { get; set; }
        public string unloadAddress { get; set; }

        public string? receiverFullName { get; set; }

        public string? receiverContacts { get; set; }

        public DateTime sendingTime { get; set; }

        public TruckType truckType { get; set; }

        public DateTime? desiredDeliveryTime { get; set; }

        public float costInRubles { get; set; }

        public Request() { }

        public Request(CreateRequestRequestDTO createRequest, Shipper shipper, bool isDelayed)
        {
            id = Guid.NewGuid();
            shipper = shipper;
            shipment = new Shipment(createRequest.shipment);
            creationTime = DateTime.Now;
            status = isDelayed ? RequestStatus.Delayed : RequestStatus.Active;
            loadCity = createRequest.loadCity;
            loadAddress = createRequest.loadAddress;
            unloadCity = createRequest.unloadCity;
            unloadAddress = createRequest.unloadAddress;
            sendingTime = createRequest.sendingTime;
            truckType = createRequest.truckType;
            desiredDeliveryTime = createRequest.desiredDeliveryTime;
            costInRubles = CostСalculator.calculateCostInRubles(createRequest.distanceBetweenCitiesInKilometers);
        }

        public void edit(EditRequestRequestDTO editRequest)
        {
            if (editRequest.unloadCity != null) unloadCity = editRequest.unloadCity;
            if (editRequest.loadCity != null) loadCity = editRequest.loadCity;
            if (editRequest.unloadAddress != null) unloadAddress = editRequest.unloadAddress;
            if (editRequest.loadAddress != null) loadAddress = editRequest.loadAddress;
            if (editRequest.sendingTime != null) sendingTime = (DateTime)editRequest.sendingTime;
            if (editRequest.desiredDeliveryTime != null) desiredDeliveryTime = editRequest.desiredDeliveryTime;
            if (editRequest.truckType != null) truckType = (TruckType)editRequest.truckType;

            if (editRequest.shipment != null)
            {
                if (editRequest.shipment.type != null) shipment.type = (ShipmentType)editRequest.shipment.type;
                if (editRequest.shipment.lengthInMeters != null) shipment.lengthInMeters = (float)editRequest.shipment.lengthInMeters;
                if (editRequest.shipment.widthInMeters != null) shipment.widthInMeters = (float)editRequest.shipment.widthInMeters;
                if (editRequest.shipment.heightInMeters != null) shipment.heightInMeters = (float)editRequest.shipment.heightInMeters;
                if (editRequest.shipment.weightInTons != null) shipment.weightInTons = (float)editRequest.shipment.weightInTons;
                if (editRequest.shipment.volumeInCubicMeters != null) shipment.volumeInCubicMeters = (float)editRequest.shipment.volumeInCubicMeters;
            }
        }
    }
    public enum RequestStatus
    {
        Active,
        Delayed,
        Accepted,
        Archived
    }
}
