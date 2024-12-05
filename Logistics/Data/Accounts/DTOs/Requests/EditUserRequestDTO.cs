using Logistics.Data.Common;
using System.ComponentModel.DataAnnotations;
namespace Logistics.Data.Account.AccountDTOs.Requests
{
    public class EditUserRequestDTO
    {
        [Annotations.FullName]
        public string? fullName { get; set; }

        [Annotations.Phone]
        public string? phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [Annotations.Email]
        public string? email { get; set; }
    }

    public class EditShipperRequestDTO : EditUserRequestDTO
    {

    }

    public class EditTransporterRequestDTO : EditUserRequestDTO
    {
        public string? permanentResidence { get; set; } 
    }
}
