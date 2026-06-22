namespace EnterpriseClaims.BuildingBlocks;

public sealed record ValidationResult(IReadOnlyCollection<ApiError> Errors)
{
    public bool IsValid => Errors.Count == 0;

    public static ValidationResult Success() => new(Array.Empty<ApiError>());

    public static ValidationResult Failure(params ApiError[] errors) => new(errors);
}
