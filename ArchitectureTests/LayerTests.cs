using ArchUnitNET.Domain;
using ArchUnitNET.xUnitV3;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests;

public class LayerTests : BaseTest
{
    private static readonly IObjectProvider<IType> DomainLayer =
        Types().That().ResideInAssembly(DomainAssembly).As("Domain layer");

    private static readonly IObjectProvider<IType> ApplicationLayer =
        Types().That().ResideInAssembly(ApplicationAssembly).As("Application layer");

    private static readonly IObjectProvider<IType> InfrastructureLayer =
        Types().That().ResideInAssembly(InfrastructureAssembly).As("Infrastructure Layer");

    private static readonly IObjectProvider<IType> WebClientLayer =
        Types().That().ResideInAssembly(WebClientAssembly).As("WebClient Layer");

    private static readonly IObjectProvider<IType> WebServerLayer =
        Types().That().ResideInAssembly(WebServerAssembly).As("WebServer Layer");

    [Fact]
    public void DomainLayer_ShouldNotDependOn_ApplicationLayer()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(ApplicationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotDependOn_InfrastructureLayer()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotDependOn_WebClientLayer()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(WebClientLayer)
            .Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotDependOn_WebServerLayer()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(WebServerLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_InfrastructureLayer()
    {
        Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_WebClientLayer()
    {
        Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAny(WebClientLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_WebServerLayer()
    {
        Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAny(WebServerLayer)
            .Check(Architecture);
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotDependOn_WebClientLayer()
    {
        Types().That().Are(InfrastructureLayer).Should()
            .NotDependOnAny(WebClientLayer)
            .Check(Architecture);
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotDependOn_WebServerLayer()
    {
        Types().That().Are(InfrastructureLayer).Should()
            .NotDependOnAny(WebServerLayer)
            .Check(Architecture);
    }
}