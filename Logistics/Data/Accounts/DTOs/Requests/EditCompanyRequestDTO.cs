using Logistics.Data.Common;
using Logistics.Data.Common.DTOs.Responses;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Data.Account.AccountDTOs.Requests
{
    public class EditCompanyRequestDTO
    {
        public required OrganizationForm organizationalForm { get; set; }

        public string? companyName { get; set; }

        [Annotations.INN]
        public string INN { get; set; }
    }
}
