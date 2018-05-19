using System;

namespace PlaygroundApi.Exceptions
{
    public class ResourceNotFoundApiException : ApiException
    {
        public ResourceNotFoundApiException(ApiErrorCode errorCode, string message, Exception innerException = null)
            : base(errorCode, message, innerException)
        {
        }
    }
}
