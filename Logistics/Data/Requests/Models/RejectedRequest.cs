namespace Logistics.Data.Requests.Models
{
    public class RejectedRequest
    {
        public Guid transporterId { get; set; }

        public Guid requestId { get; set; }

        public RejectedRequest()
        {
            
        }
        public RejectedRequest(Guid transporterId, Guid requestId)
        {
            this.transporterId = transporterId;
            this.requestId = requestId;
        }
    }
}
