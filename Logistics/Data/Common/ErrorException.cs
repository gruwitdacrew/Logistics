﻿namespace Logistics.Data.Common
{
    public class ErrorException : Exception
    {
        public int status { get; set; }

        public string message { get; set; }

        public ErrorException(int status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }

    public class ErrorCollectionException : Exception
    {
        public int status { get; set; }

        public List<string> errors { get; set; }

        public ErrorCollectionException(int status, List<string> errors)
        {
            this.status = status;
            this.errors = errors;
        }
    }
}
