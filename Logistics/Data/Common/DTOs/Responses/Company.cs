using Logistics.Data.Account.AccountDTOs.Requests;
using Logistics.Data.Account.Models;
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

        public void edit(EditCompanyRequestDTO editRequest)
        {
            organizationalForm = editRequest.organizationalForm;
            if (editRequest.companyName != null) companyName = editRequest.companyName;
            INN = editRequest.INN;
        }
    }

    public enum OrganizationForm
    {
        OOO,
        IP,
        Individual
    }
}
