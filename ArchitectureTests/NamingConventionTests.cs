using ArchUnitNET.xUnitV3;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests;

public class NamingConventionTests : BaseTest
{
    [Fact]
    public void CommandHandlersAndQueryHandlers_ShouldHave_NameEndingWith_CommandHandler()
    {
        Classes().That()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Or()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should().HaveNameEndingWith("Handler")
            .Check(Architecture);
    }


    [Fact]
    public void EntityConfigurations_ShouldHaveNameEndingWith_Configuration()
    {
        Classes().That()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .Should().HaveNameEndingWith("Configuration")
            .Check(Architecture);
    }
}