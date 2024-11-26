using Logistics.Data.Account.Models;
using Logistics.Data.Documents.DTOs.Requests;

namespace Logistics.Data.Documents.Models
{
    public class DriverLicense
    {
        public Guid id { get; set; }

        public string series { get; set; }

        public string number { get; set; }

        public byte[]? scan { get; set; }

        public Transporter transporter { get; set; }

        public DriverLicense(){}

        public DriverLicense(CreateDriverLicenseRequestDTO createDriverLicense, Transporter transporter)
        {
            id = Guid.NewGuid();
            series = createDriverLicense.series;
            number = createDriverLicense.number;
            this.transporter = transporter;
        }

        public void edit(EditDriverLicenseDTO editLicense)
        {
            if (editLicense.series != null) series = editLicense.series;
            if (editLicense.number != null) number = editLicense.number;
        }
    }
}
