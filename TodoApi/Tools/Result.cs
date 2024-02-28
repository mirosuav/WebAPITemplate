using System.Runtime.CompilerServices;
using System.Text.Json;

namespace TodoApi.Tools;

/// <summary>
/// Optional Result of type T. Is either sucessfull 'Value' object of type T or 'Error'
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly record struct Result<T> : IResult
{
    /// <summary>
    /// No result value and no Error either
    /// </summary>
    public static readonly Result<T> Empty = new();

    public readonly bool IsSuccess;
    public readonly T? Value;
    public readonly Error Error;
    public bool IsFaulted => !IsSuccess;

    public Result()
    {
        IsSuccess = true;
        Value = default;
        Error = Error.None;
    }

    public Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = Error.None;
    }

    public Result(Error error)
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

    public static implicit operator Result<T>(Error error) => Failure(error);

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);

    public string AsJson()
        => IsSuccess
        ? JsonSerializer.Serialize(Value)
        : JsonSerializer.Serialize(Error);
}
