namespace Logistics.Data.Common.CommonDTOs.Responses
{
    public class ErrorProblemDetails
    {
        public int status { get; set; }
        public Dictionary<string, string> errors { get; set; }

        public ErrorProblemDetails(int status) {
            this.status = status;
            this.errors = new Dictionary<string, string>();
        }
        public ErrorProblemDetails(int status, Dictionary<string, string> errors)
        {
            this.status = status;
            this.errors = errors;
        }

        public void addError(string key, string error)
        {
            errors.Add(key, error);
        }
    }
}
