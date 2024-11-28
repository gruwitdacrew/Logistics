using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logistics.Data.Common.DTOs.Responses
{
    [Owned]
    public class Company
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
