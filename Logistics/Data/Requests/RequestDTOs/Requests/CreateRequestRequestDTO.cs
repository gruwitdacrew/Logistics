using Logistics.Data.Account.Models;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Data.Requests.RequestDTOs.Requests
{
    public class CreateRequestRequestDTO
    {
        public string loadCity { get; set; }
        public string loadAddress { get; set; }

        public int distanceBetweenCitiesInKilometers { get; set; }

        public string unloadCity { get; set; }
        public string unloadAddress { get; set; }

        public DateTime sendingTime { get; set; }

        public DateTime desiredDeliveryTime { get; set; }

        public CreateShipmentRequestDTO shipment { get; set; }

        public TruckType truckType { get; set; }
    }
}
