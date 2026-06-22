using Claims.Api.Domain;
using NetArchTest.Rules;

namespace EnterpriseClaims.ArchitectureTests;

public class LayerTests
{
    [Fact]
    public void DomainLayer_ShouldNot_HaveDependencyOnInfrastructure()
    {
        var result = Types.InAssembly(typeof(ClaimRecord).Assembly)
            .That()
            .ResideInNamespace("Claims.Api.Domain")
            .ShouldNot()
            .HaveDependencyOn("Claims.Api.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
    
    [Fact]
    public void DomainLayer_ShouldNot_HaveDependencyOnData()
    {
        var result = Types.InAssembly(typeof(ClaimRecord).Assembly)
            .That()
            .ResideInNamespace("Claims.Api.Domain")
            .ShouldNot()
            .HaveDependencyOn("Claims.Api.Data")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}
