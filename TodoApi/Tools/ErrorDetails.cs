using System.Diagnostics.Tracing;

namespace TodoApi.Tools;

public record ErrorDetails
{
    public static readonly ErrorDetails None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly ErrorDetails NullValue = new("Error.NullValue", "Null or missing value provided.", ErrorType.Failure);
    public static readonly ErrorDetails ServerError = new("Error.Server", "Internal server error", ErrorType.Failure);

    private ErrorDetails(string code, string description, ErrorType errorType)
    {
        Code = code;
        Description = description;
        ErrorType = errorType;
    }

    public string Code { get; }
    public string Description { get; }
    public ErrorType ErrorType { get; }

    public static ErrorDetails Failure(string code, string description) => new(code, description, ErrorType.Failure);
    public static ErrorDetails Validation(string code, string description) => new(code, description, ErrorType.Validation);
    public static ErrorDetails NotFound(string code, string description) => new(code, description, ErrorType.NotFound);
    public static ErrorDetails Conflict(string code, string description) => new(code, description, ErrorType.Conflict);

}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3
}
