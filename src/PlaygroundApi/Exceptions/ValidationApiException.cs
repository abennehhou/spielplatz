using System;

namespace PlaygroundApi.Exceptions
{
    public class ValidationApiException : ApiException
    {
        public ValidationApiException(ApiErrorCode errorCode, string message, Exception innerException = null)
            : base(errorCode, message, innerException)
        {
        }
    }
}
