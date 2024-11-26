
using Logistics.Data.Common;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Data.Account.AccountDTOs.Requests
{
    public class ChangePasswordRequestDTO
    {
        [Annotations.Password]
        public string password { get; set; }

        public string oldPassword { get; set; }
    }
}
