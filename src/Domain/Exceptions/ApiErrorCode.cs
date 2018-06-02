namespace PlaygroundApi.Domain.Exceptions
{
    public enum ApiErrorCode
    {
        InternalError,
        MissingInformation,
        InvalidInformation,
        ValueNotFound,
        DeleteValueForbidden,
        ItemNotFound
    }
}
