using System;

namespace PlaygroundApi.Exceptions
{
    public class ApiException : Exception
    {
        public ApiErrorCode ApiErrorCode { get; set; }

        public ApiException(ApiErrorCode errorCode, string message, Exception innerException = null)
            : base(message, innerException)
        {
            ApiErrorCode = errorCode;
        }
    }
}
