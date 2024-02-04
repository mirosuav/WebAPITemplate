using System.Runtime.CompilerServices;
using System.Text.Json;

namespace TodoApi.Tools;
public readonly record struct Result<T>
{
    public static readonly Result<T> Empty = new();

    public readonly bool IsSuccess;
    public readonly T? Value;
    public readonly ErrorDetails Error;
    public bool IsFaulted => !IsSuccess;

    public Result()
    {
        IsSuccess = true;
        Value = default;
        Error = ErrorDetails.None;
    }

    public Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = ErrorDetails.None;
    }

    public Result(ErrorDetails error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public override string ToString()
        => IsSuccess
        ? Value?.ToString() ?? "(null)"
        : Error!.ToString();

    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator Result<T>(ErrorDetails error) => Failure(error);

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(ErrorDetails error) => new(error);

    public string AsJson()
        => IsSuccess
        ? JsonSerializer.Serialize(Value)
        : JsonSerializer.Serialize(Error);
}
