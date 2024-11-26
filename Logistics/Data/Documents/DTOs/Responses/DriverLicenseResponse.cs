using Logistics.Data.Documents.Models;

namespace Logistics.Data.Documents.DTOs.Responses
{
    public class DriverLicenseResponse
    {
        public string series { get; set; }

        public string number { get; set; }

        public bool scan { get; set; }

        public DriverLicenseResponse(DriverLicense license)
        {
            series = license.series;
            number = license.number;
            scan = license.scan != null;
        }
    }
}
