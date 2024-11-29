using Logistics.Data.Common;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Logistics.Data.Documents.DTOs.Requests
{
    public class EditPassportDTO
    {
        [Annotations.Series]
        public string? series { get; set; }

        [Annotations.PassportNumber]
        public string? number { get; set; }

        public string? issuedBy { get; set; }

        [Annotations.Code]
        public string? code { get; set; }

        public DateTime? dateOfIssue { get; set; }
    }
}
