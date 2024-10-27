namespace Application.Common;

public class ApiResponse<T>
{
    public T? Data { get; private set; }
    public bool Success { get; private set; }
    public string Message { get; private set; } = string.Empty;

    private ApiResponse(T? data, bool success, string message)
    {
        Data = data;
        Success = success;
        Message = message;
    }

    public static ApiResponse<T> Sucess(T data) => new(data, true, string.Empty);

    public static ApiResponse<T> Failure(string errorMessage) => new(default, false, errorMessage);
}
