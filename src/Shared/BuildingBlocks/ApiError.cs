namespace EnterpriseClaims.BuildingBlocks;

public sealed record ApiError(string Code, string Message)
{
    public static ApiError Validation(string code, string message) => new(code, message);

    public static ApiError NotFound(string code, string message) => new(code, message);
}
