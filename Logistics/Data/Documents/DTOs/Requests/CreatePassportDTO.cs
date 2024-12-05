using Logistics.Data.Common;

namespace Logistics.Data.Documents.DTOs.Requests
{
    public class CreatePassportDTO
    {
        [Annotations.Series]
        public string series { get; set; }

        [Annotations.PassportNumber]
        public string number { get; set; }

        public string issuedBy { get; set; }

        [Annotations.Code]
        public string code { get; set; }

        [Annotations.Date]
        public string dateOfIssue { get; set; }
    }
}
