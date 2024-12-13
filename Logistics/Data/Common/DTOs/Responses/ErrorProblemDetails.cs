namespace Logistics.Data.Common.CommonDTOs.Responses
{
    public class ErrorProblemDetails
    {
        public int status { get; set; }
        public List<string> errors { get; set; }

        public ErrorProblemDetails(int status) {
            this.status = status;
            this.errors = new List<string>();
        }
        public ErrorProblemDetails(int status, List<string> errors)
        {
            this.status = status;
            this.errors = errors;
        }

        public void addError(string error)
        {
            errors.Add(error);
        }
    }
}
