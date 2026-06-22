namespace Claims.Api.Application;

public sealed class ClaimNumberGenerator
{
    public string Create() => $"CLM-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
}
