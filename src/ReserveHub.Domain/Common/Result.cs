using System.Diagnostics.CodeAnalysis;

namespace ReserveHub.Domain.Common;

public class Result
{
    protected Result(Error? error)
    {
        Error = error;
    }

    public Error? Error { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => Error is not null;

    public static Result Success() => new(null);

    public static implicit operator Result(Error error) => new(error);
}

public sealed class Result<T> : Result
{
    private readonly T? _value;

    private Result(T value) : base(null)
    {
        _value = value;
    }

    private Result(Error error) : base(error)
    {
    }

    public T Value => IsFailure
        ? throw new InvalidOperationException("A failed result has no value.")
        : _value!;

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Error error) => new(error);
}
