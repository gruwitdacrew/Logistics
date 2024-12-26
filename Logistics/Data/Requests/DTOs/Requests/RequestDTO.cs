using Logistics.Data.Account.Models;
using Logistics.Data.Common;
using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Requests.Models;

namespace Logistics.Data.Requests.DTOs.Requests
{
    public class RequestDTO
    {
        public City loadCity { get; set; }
        public string loadAddress { get; set; }

        public int distanceBetweenCitiesInKilometers { get; set; }

        public City unloadCity { get; set; }
        public string unloadAddress { get; set; }


        public DateTime? sendingTimeFrom { get; set; }
        public DateTime sendingTime { get; set; }

        public ShipmentDTO shipment { get; set; }

        public TruckType truckType { get; set; }

        public void validate()
        {
            ErrorProblemDetails validationProblems = new ErrorProblemDetails(400);

            if (sendingTime <= sendingTimeFrom) validationProblems.addError("sendingTimeFrom", "Дата с должна быть меньше даты по");

            if (validationProblems.errors.Count > 0) throw new ErrorCollectionException(validationProblems.status, validationProblems.errors);
        }
    }
}
