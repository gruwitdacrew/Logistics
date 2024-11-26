using Logistics.Data.Accounts.Models;
using Logistics.Data.Common;

namespace Logistics.Data.Account.AccountDTOs.Requests
{
    public class EditCompanyRequestDTO
    {
        public OrganizationForm organizationalForm { get; set; }

        public string? companyName { get; set; }

        [Annotations.INN]
        public string INN { get; set; }
    }
}
