using System.ComponentModel.DataAnnotations;

namespace Logistics.Data.Account.Models
{
    public class PendingEmail
    {
        public Guid id { get; set; }

        public User user { get; set; }

        public string value { get; set; }

        public PendingEmail() { }

        public PendingEmail(User user, string value)
        {
            id = Guid.NewGuid();
            this.user = user;
            this.value = value;
        }
    }
}
