using Microsoft.EntityFrameworkCore;

namespace Logistics.Data.Documents.Models
{
    [Owned]
    public class Scan
    {
        public string fileName { get; set; }

        public byte[] data { get; set; }

        public Scan(string fileName, byte[] data)
        {
            this.fileName = fileName;
            this.data = data;   
        }
    }
}
