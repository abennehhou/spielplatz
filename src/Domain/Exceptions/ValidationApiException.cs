using System;

namespace PlaygroundApi.Domain.Exceptions
{
    public class ValidationApiException : ApiException
    {
        public ValidationApiException(ApiErrorCode errorCode, string message, Exception innerException = null)
            : base(errorCode, message, innerException)
        {
        }
    }
}
