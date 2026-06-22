using EnterpriseClaims.BuildingBlocks.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseClaims.UnitTests.Security;

public class SecurityExtensionsTests
{
    [Fact]
    public async Task AddEnterpriseSecurity_RegistersRequiredPolicies()
    {
        // Arrange
        var services = new ServiceCollection();
        
        var configData = new Dictionary<string, string?>
        {
            { "Jwt:Key", "this_is_a_test_key_that_is_long_enough_12345!" }
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(configData).Build();

        // Act
        services.AddEnterpriseSecurity(configuration);
        var provider = services.BuildServiceProvider();
        var policyProvider = provider.GetRequiredService<IAuthorizationPolicyProvider>();

        // Assert
        Assert.NotNull(await policyProvider.GetPolicyAsync("Customer"));
        Assert.NotNull(await policyProvider.GetPolicyAsync("ClaimProcessor"));
        Assert.NotNull(await policyProvider.GetPolicyAsync("Supervisor"));
        Assert.NotNull(await policyProvider.GetPolicyAsync("Admin"));
    }
}
