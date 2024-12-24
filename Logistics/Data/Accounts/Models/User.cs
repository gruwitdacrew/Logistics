using Logistics.Data.Account.AccountDTOs.Requests;
using Logistics.Data.Common.DTOs.Responses;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Logistics.Data.Account.Models
{
    public class User
    {

        [Key]
        public Guid id { get; set; }

        public string fullName { get; set; }

        public string phone { get; set; }

        public string? email { get; set; }

        public byte[]? photo { get; set; }

        public Company company { get; set; }

        public Role role { get; set; }

        public string? password { get; set; }

        public string? token { get; set; }

        public User() { }

        public User(RegisterRequestDTO registerRequest)
        {
            id = Guid.NewGuid();
            fullName = registerRequest.fullName;
            phone = registerRequest.phone;
            password = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(registerRequest.password)));
        }

        public void edit(EditUserRequestDTO editUserRequest)
        {
            fullName = editUserRequest.fullName;
            phone = editUserRequest.phone;
        }

        public bool haveFilledInCompany()
        {
            if (company.INN == null) return false;
            if (company.organizationalForm == null) return false;
            if (company.companyName == null && company.organizationalForm != OrganizationForm.Individual) return false;
            return true;
        }
    }
    public enum Role
    {
        Shipper,
        Transporter
    }
}
