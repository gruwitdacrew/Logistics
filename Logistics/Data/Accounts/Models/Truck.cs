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

        public string regionCode { get; set; }

        public float lengthInMeters { get; set; }

        public float widthInMeters { get; set; }

        public float heightInMeters { get; set; }

        public Truck() {}
    }

    public enum TruckType
    {
        Tented
    }
}
