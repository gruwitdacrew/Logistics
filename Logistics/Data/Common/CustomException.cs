namespace Logistics.Data.Common
{
    public class CustomException : Exception
    {
        public int status { get; set; }

        public string message { get; set; }

        public CustomException(int status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }
}
