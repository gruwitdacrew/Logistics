using Logistics.Data.Account.Models;

namespace Logistics.Data.Transportations.Models
{
    public class Review
    {
        public Guid transportationId { get; set; }

        public Guid reviewerId { get; set; }

        public Guid userId { get; set; }

        public string value { get; set; }

        public Review() {}

        public Review(Guid transportationId, Guid reviewerId, Guid userId, string value)
        {
            this.transportationId = transportationId;
            this.reviewerId = reviewerId;
            this.userId = userId;
            this.value = value;
        }
    }
}
