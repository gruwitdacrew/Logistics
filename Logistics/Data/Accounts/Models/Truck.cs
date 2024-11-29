using Logistics.Data.Accounts.DTOs.Requests;

namespace Logistics.Data.Account.Models
{
    public class Truck
    {
        public Guid id { get; set; }

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

        public Truck() { }

        public Truck(CreateTruckRequestDTO createRequest)
        {
            id = Guid.NewGuid();
            carBrand = createRequest.carBrand;
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
            if (editRequest.carBrand != null) carBrand = editRequest.carBrand;
            if (editRequest.model != null) model = editRequest.model;
            if (editRequest.truckType != null) truckType = (TruckType)editRequest.truckType;
            if (editRequest.loadCapacityInTons != null) loadCapacityInTons = (int)editRequest.loadCapacityInTons;
            if (editRequest.yearOfProduction != null) yearOfProduction = (int)editRequest.yearOfProduction;
            if (editRequest.regionCode != null) regionCode = (int)editRequest.regionCode;
            if (editRequest.lengthInMeters != null) lengthInMeters = (float)editRequest.lengthInMeters;
            if (editRequest.widthInMeters != null) widthInMeters = (float)editRequest.widthInMeters;
            if (editRequest.heightInMeters != null) heightInMeters = (float)editRequest.heightInMeters;

        }
    }
    public enum TruckType
    {
        Tented
    }
}
