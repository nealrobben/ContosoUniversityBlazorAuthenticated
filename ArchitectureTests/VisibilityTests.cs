using ArchUnitNET.xUnitV3;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests;

public class VisibilityTests : BaseTest
{
    [Fact]
    public void CommandHandlers_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Or()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should().BeInternal()
            .Check(Architecture);
    }

    [Fact]
    public void EntityConfigurations_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .Should().BeInternal()
            .Check(Architecture);
    }
}