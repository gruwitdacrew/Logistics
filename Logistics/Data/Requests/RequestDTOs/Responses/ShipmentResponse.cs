using Logistics.Data.Requests.Models;

namespace Logistics.Data.Requests.RequestDTOs.Responses
{
    public class ShipmentResponse
    {
        public ShipmentType type { get; set; }

        public float lengthInMeters { get; set; }

        public float widthInMeters { get; set; }

        public float heightInMeters { get; set; }

        public float weightInTons { get; set; }

        public float volumeInCubicMeters { get; set; }

        public ShipmentResponse(Shipment shipment)
        {
            type = shipment.type;
            lengthInMeters = shipment.lengthInMeters;
            widthInMeters = shipment.widthInMeters;
            heightInMeters = shipment.heightInMeters;
            weightInTons = shipment.weightInTons;
            volumeInCubicMeters = shipment.volumeInCubicMeters;
        }
    }
}
