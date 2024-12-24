using Logistics.Data.Accounts.DTOs.Requests;

namespace Logistics.Data.Account.Models
{
    public class Truck
    {
        public Guid id { get; set; }

        public Guid transporterId { get; set; }

        public TruckBrand truckBrand { get; set; }

        public string model { get; set; }

        public TruckType truckType { get; set; }

        public int loadCapacityInTons { get; set; }

        public int yearOfProduction { get; set; }

        public string number { get; set; }

        public int regionCode { get; set; }

        public float lengthInMeters { get; set; }

        public float widthInMeters { get; set; }

        public float heightInMeters { get; set; }

        public Truck() { }

        public Truck(CreateTruckRequestDTO createRequest)
        {
            id = Guid.NewGuid();
            truckBrand = createRequest.truckBrand;
            model = createRequest.model;
            truckType = createRequest.truckType;
            loadCapacityInTons = createRequest.loadCapacityInTons;
            yearOfProduction = createRequest.yearOfProduction;
            number = createRequest.number;
            regionCode = createRequest.regionCode;
            lengthInMeters = createRequest.lengthInMeters;
            widthInMeters = createRequest.widthInMeters;
            heightInMeters = createRequest.heightInMeters;
        }

        public void edit(EditTruckRequestDTO editRequest)
        {
            truckBrand = editRequest.truckBrand;
            model = editRequest.model;
            truckType = editRequest.truckType;
            loadCapacityInTons = editRequest.loadCapacityInTons;
            yearOfProduction = editRequest.yearOfProduction;
            regionCode = editRequest.regionCode;
            lengthInMeters = editRequest.lengthInMeters;
            widthInMeters = editRequest.widthInMeters;
            heightInMeters = editRequest.heightInMeters;

        }
    }
    public enum TruckType
    {
        Tented,
        Flatbed,
        Curtain,
        CurtainFlatbed,
        Dump
    }

    public enum TruckBrand
    {
        Volvo,
        Scania,
        MercedesBenz,
        MAN,
        DAF,
        Iveco,
        Freightliner,
        Kenworth,
        Peterbilt,
        International,
        Isuzu,
        MitsubishiFuso,
        Hino,
        KamAZ,
        GAZ,
        Ural
    }
}
