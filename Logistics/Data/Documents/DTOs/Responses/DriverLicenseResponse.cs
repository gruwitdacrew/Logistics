using Logistics.Data.Documents.Models;

namespace Logistics.Data.Documents.DTOs.Responses
{
    public class DriverLicenseResponse
    {
        public string series { get; set; }

        public string number { get; set; }

        public string? scanFileName { get; set; }

        public DriverLicenseResponse(DriverLicense license)
        {
            series = license.series;
            number = license.number;
            scanFileName = license.scan?.fileName;
        }
    }
}
