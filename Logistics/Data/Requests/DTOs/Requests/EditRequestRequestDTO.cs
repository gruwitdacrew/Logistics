using Logistics.Data.Account.Models;
namespace Logistics.Data.Requests.DTOs.Requests
{
    public class EditRequestRequestDTO
    {
        public string? loadCity { get; set; }
        public string? loadAddress { get; set; }

        public int? distanceBetweenCitiesInKilometers { get; set; }

        public string? unloadCity { get; set; }
        public string? unloadAddress { get; set; }

        public DateTime? sendingTime { get; set; }

        public DateTime? desiredDeliveryTime { get; set; }

        public EditShipmentRequestDTO? shipment { get; set; }

        public TruckType? truckType { get; set; }
    }
}
