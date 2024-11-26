namespace Logistics.Data.Common.CommonDTOs.Responses
{
    public class CustomProblemDetails
    {
        public int? status { get; set; }
        public IDictionary<string, string[]> errors { get; set; }
    }
}
