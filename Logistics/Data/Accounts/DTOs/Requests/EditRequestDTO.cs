using Logistics.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Logistics.Data.Account.AccountDTOs.Requests
{
    public class EditRequestDTO
    {
        [Annotations.FullName]
        public string? fullName { get; set; }

        [Annotations.FullName]
        public string? phone { get; set; }
        
        [Annotations.Email]
        public string? email { get; set; }
    }
}
