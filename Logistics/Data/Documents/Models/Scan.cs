using Microsoft.EntityFrameworkCore;

namespace Logistics.Data.Documents.Models
{
    [Owned]
    public class Scan
    {
        public string fileName { get; set; }

        public Guid fileId { get; set; }

        public Scan(string fileName, Guid fileId)
        {
            this.fileName = fileName;
            this.fileId = fileId;   
        }
    }
}
