namespace Application.Common;

public class Result<T>
{
    public T? Value { get; private set; }
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; } = string.Empty;

    private Result(T? data, bool success, string message)
    {
        Value = data;
        IsSuccess = success;
        Message = message;
    }

    public static Result<T> Success(T result) => new(result, true, string.Empty);

    public static Result<T> Failure(string errorMessage) => new(default, false, errorMessage);
}
