using System.Linq;
using Domain.Entities.Projections.Departments;
using WebUI.Client.Dtos.Departments;

namespace WebUI.Server.Mappers;

public static class DepartmentDtoMapper
{
    public static DepartmentLookupDto ToDto(DepartmentLookup model)
    {
        return new DepartmentLookupDto(model.DepartmentID, model.Name);
    }

    public static DepartmentsLookupDto ToDto(DepartmentsLookup model)
    {
        return new DepartmentsLookupDto
        {
            Departments = model.Departments.Select(ToDto).ToList()
        };
    }

    public static DepartmentDetailDto ToDto(DepartmentDetail model)
    {
        return new DepartmentDetailDto(model.DepartmentID, model.Name, model.Budget, model.StartDate, model.AdministratorName, model.InstructorID, model.RowVersion);
    }

    public static DepartmentOverviewDto ToDto(DepartmentOverview model)
    {
        return new DepartmentOverviewDto(model.DepartmentID, model.Name, model.Budget, model.StartDate, model.AdministratorName);
    }
}
