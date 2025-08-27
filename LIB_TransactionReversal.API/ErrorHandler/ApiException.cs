using System;

namespace CUSTOR.API.ExceptionFilter
{
    public class ApiException : Exception
    {
        public ApiException(string message,
            int statusCode = 500,
            ValidationErrorCollection errors = null) :
            base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        public ApiException(Exception ex, int statusCode = 500) : base(ex.Message)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }

        public ValidationErrorCollection Errors { get; set; }
    }
}