using Logistics.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Data.Documents.Models
{
    public class EditDriverLicenseDTO
    {
        [Annotations.Series]
        public string series { get; set; }

        [Annotations.PassportNumber]
        public string number { get; set; }
    }
}
