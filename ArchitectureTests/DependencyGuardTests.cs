using ArchUnitNET.xUnitV3;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchitectureTests;

public class DependencyGuardTests : BaseTest
{
    [Fact]
    public void DomainLayer_ShouldNotDependOn_EntityFramework()
    {
        Types().That().ResideInAssembly(DomainAssembly).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespace("Microsoft.EntityFrameworkCore")
            .Check(Architecture);
    }

    //TODO: Let interface for ISchoolContext use IQueryable instead of DBSet?
    //[Fact]
    //public void ApplicationLayer_ShouldNotDependOn_EntityFramework()
    //{
    //    Types().That().ResideInAssembly(ApplicationAssembly).Should()
    //        .NotDependOnAnyTypesThat()
    //        .ResideInNamespace("Microsoft.EntityFrameworkCore")
    //        .Check(Architecture);
    //}
}