namespace EnterpriseClaims.BuildingBlocks;

public sealed record ApiResponse<T>(
    bool Succeeded,
    T? Data,
    IReadOnlyCollection<ApiError> Errors)
{
    public static ApiResponse<T> Success(T data) => new(true, data, Array.Empty<ApiError>());

    public static ApiResponse<T> Failure(IReadOnlyCollection<ApiError> errors) => new(false, default, errors);
}

public static class ApiResponse
{
    public static ApiResponse<T> Success<T>(T data) => ApiResponse<T>.Success(data);

    public static ApiResponse<T> Failure<T>(IReadOnlyCollection<ApiError> errors) => ApiResponse<T>.Failure(errors);
}
