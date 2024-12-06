using Logistics.Data.Account.Models;
using Logistics.Data.Common;
using Logistics.Services.Utils;

namespace Logistics.Data.Accounts.DTOs.Responses
{
    public class TruckResponse
    {
        public string carBrand { get; set; }

        public string model { get; set; }

        public TruckType truckType { get; set; }

        public int loadCapacityInTons { get; set; }

        public int yearOfProduction { get; set; }

        public string number { get; set; }

        public int regionCode { get; set; }

        public float lengthInMeters { get; set; }

        public float widthInMeters { get; set; }

        public float heightInMeters { get; set; }

        public TruckResponse(Truck truck)
        {
            carBrand = truck.carBrand;
            model = truck.model;
            truckType = truck.truckType;
            loadCapacityInTons = truck.loadCapacityInTons;
            yearOfProduction = truck.yearOfProduction;
            number = truck.number;
            regionCode = truck.regionCode;
            lengthInMeters = truck.lengthInMeters;
            widthInMeters = truck.widthInMeters;
            heightInMeters = truck.heightInMeters;
        }
    }
}
