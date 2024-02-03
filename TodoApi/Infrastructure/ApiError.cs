using System.Diagnostics.Tracing;

namespace TodoApi.Infrastructure;

public record ApiError
{
    public static readonly ApiError None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly ApiError NullValue = new("Error.NullValue", "Null or missing value provided.", ErrorType.Failure);
    public static readonly ApiError ServerError = new("Error.Server", "Internal server error", ErrorType.Failure);

    private ApiError(string code, string description, ErrorType errorType)
    {
        Code = code;
        Description = description;
        ErrorType = errorType;
    }

    public string Code { get; }
    public string Description { get; }
    public ErrorType ErrorType { get; }

    public static ApiError Failure(string code, string description) => new(code, description, ErrorType.Failure);
    public static ApiError Validation(string code, string description) => new(code, description, ErrorType.Validation);
    public static ApiError NotFound(string code, string description) => new(code, description, ErrorType.NotFound);
    public static ApiError Conflict(string code, string description) => new(code, description, ErrorType.Conflict);

}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3
}
