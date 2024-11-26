using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logistics.Data.Accounts.Models
{
    [Owned]
    public class AboutCompany
    {
        [Column("organizationalForm")]
        public OrganizationForm? organizationalForm { get; set; }

        [Column("companyName")]
        public string? companyName { get; set; }

        [Column("INN")]
        public string? INN { get; set; }
    }

    public enum OrganizationForm
    {
        OOO,
        IP,
        Individual
    }
}
