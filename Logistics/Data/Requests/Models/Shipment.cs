using Logistics.Data.Requests.DTOs.Requests;

namespace Logistics.Data.Requests.Models
{
    public class Shipment
    {
        public Guid id { get; set; }

        public Guid requestId { get; set; }

        public ShipmentType type { get; set; }

        public float lengthInMeters { get; set; }

        public float widthInMeters { get; set; }

        public float heightInMeters { get; set; }

        public float weightInTons { get; set; }


        public Shipment() { }

        public Shipment(CreateShipmentRequestDTO createShipment, Guid requestId)
        {
            id = Guid.NewGuid();
            this.requestId = requestId;
            type = createShipment.type;
            lengthInMeters = createShipment.lengthInMeters;
            widthInMeters = createShipment.widthInMeters;
            heightInMeters = createShipment.heightInMeters;
            weightInTons = createShipment.weightInTons;
        }
    }

    public enum ShipmentType
    {
        Perishable,
        Bulk,
        Piece,
        Oversized,
        Gaseous,
        Dusty,
        Liquid,
        Dangerous
    }
}
