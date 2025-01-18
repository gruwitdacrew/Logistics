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

    public class CompanyResponse
    {
        public OrganizationForm organizationalForm { get; set; }

        public string companyName { get; set; }

        public string INN { get; set; }

        public string phone { get; set; }

        public CompanyResponse(User user)
        {
            organizationalForm = (OrganizationForm)user.company.organizationalForm;
            if (user.company.organizationalForm == OrganizationForm.Individual)
            {
                companyName = user.fullName;
            }
            else
            {
                companyName = user.company.companyName;
            }
            INN = user.company.INN;
        }
    }

    public class CompanyResponseWithPhone : CompanyResponse
    {
        public string phone { get; set; }

        public CompanyResponseWithPhone(User user) : base(user)
        {
            phone = user.phone;
        }
    }



    public enum OrganizationForm
    {
        OOO,
        IP,
        Individual
    }
}
