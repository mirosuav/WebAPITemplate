namespace TodoApi.Tools;

/// <summary>
/// Operation Error
/// </summary>
/// <param name="Code">Short error code</param>
/// <param name="Description">Error human readable description</param>
/// <param name="ErrorType">Error type</param>
public record Error(string Code, string Description, ErrorType ErrorType)
{
    public static readonly Error None = Failure(string.Empty, string.Empty);
    public static readonly Error NullValue = Failure("Error.NullValue", "Null or missing value provided.");
    public static readonly Error ServerError = Failure("Error.Server", "Internal server error");
    public static readonly Error OperationCancelled = Failure("Error.Cancelled", "Operation cancelled");

    public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);
    public static Error Validation(string code, string description) => new(code, description, ErrorType.Validation);
    public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);
    public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3
}
