namespace AuctionAPI.Util;

public class Result<T>
{
    private Result(bool isSuccess, T? value, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors.ToList().AsReadOnly();
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public IReadOnlyList<string> Errors { get; }

    public static Result<T> Success(T value)
        => new(true, value, Array.Empty<string>());

    public static Result<T> Failure(string error)
        => new(false, default, new[] { error });

    public static Result<T> Failure(IEnumerable<string> errors)
        => new(false, default, errors);

    public void Match(Action<T> ifSuccess, Action<IReadOnlyList<string>> ifFailure)
    {
        if (IsSuccess)  
            ifSuccess(Value!);
        else
            ifFailure(Errors);
    }

    public void IfFailure(Action<IReadOnlyList<string>> action)
    {
        action(Errors);
    }

    // Implicit operator for success
    public static implicit operator Result<T>(T value) => Success(value);

    // Implicit operator for single error failure
    public static implicit operator Result<T>(string error) => Failure(error);

    // Implicit operator for multiple error failures
    public static implicit operator Result<T>(List<string> errors) => Failure(errors);

    public static implicit operator Result<T>(string[] errors) => Failure(errors);
}

public static class Result
{
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
    public static Result<T> Failure<T>(string error) => Result<T>.Failure(error);
    public static Result<T> Failure<T>(IEnumerable<string> errors) => Result<T>.Failure(errors);
}
