using Logistics.Data.Documents.Models;

namespace Logistics.Data.Documents.DTOs.Responses
{
    public class PassportResponse
    {
        public string series { get; set; }

        public string number { get; set; }

        public string issuedBy { get; set; }

        public string code { get; set; }

        public string dateOfIssue { get; set; }

        public string? scanFileName { get; set; }

        public PassportResponse(Passport passport)
        {
            series = passport.series;
            number = passport.number;
            issuedBy = passport.issuedBy;
            code = passport.code;
            dateOfIssue = passport.dateOfIssue;
            scanFileName = passport.scan?.fileName;
        }
    }
}
