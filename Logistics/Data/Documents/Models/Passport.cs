using Logistics.Data.Account.Models;
using Logistics.Data.Documents.DTOs.Requests;

namespace Logistics.Data.Documents.Models
{
    public class Passport
    {
        public Guid id { get; set; }

        public Guid userId { get; set; }

        public string series { get; set; }

        public string number { get; set; }

        public string issuedBy { get; set; }

        public string code { get; set; }

        public string dateOfIssue { get; set; }

        public Scan? scan { get; set; }

        public User user { get; set; }

        public Passport(){}

        public Passport(CreatePassportDTO createPassport, User user)
        {
            id = Guid.NewGuid();
            series = createPassport.series;
            number = createPassport.number;
            issuedBy = createPassport.issuedBy;
            code = createPassport.code;
            dateOfIssue = createPassport.dateOfIssue;
            this.user = user;
        }

        public void edit(EditPassportDTO editPassport)
        {
            if (editPassport.series != null) series = editPassport.series;
            if (editPassport.number != null) number = editPassport.number;
            if (editPassport.issuedBy != null) issuedBy = editPassport.issuedBy;
            if (editPassport.code != null) code = editPassport.code;
            if (editPassport.dateOfIssue != null) dateOfIssue = editPassport.dateOfIssue;
        }

        public bool haveScan()
        {
            return scan != null;
        }
    }
}
