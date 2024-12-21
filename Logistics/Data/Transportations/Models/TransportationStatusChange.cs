namespace Logistics.Data.Transportations.Models
{
    public class TransportationStatusChange
    {
        public Guid id { get; set; }

        public Guid transportationId { get; set; }

        public Transportation transportation { get; set; }

        public TransportationStatus status { get; set; }

        public DateTime time { get; set; }

        public TransportationStatusChange(){}

        public TransportationStatusChange(Transportation transportation, TransportationStatus status)
        {
            id = Guid.NewGuid();
            this.transportation = transportation;
            this.status = status;
            time = DateTime.UtcNow;
        }
    }
}
