using ContosoUniversityBlazor.Domain.Entities;
using Domain.Entities.Projections.Departments;
using System.Linq;
using WebUI.Client.Dtos.Departments;

namespace WebUI.Server.Mappers;

public static class DepartmentDtoMapper
{
    public static DepartmentLookupDto ToDto(DepartmentLookup model)
    {
        return new DepartmentLookupDto
        {
            DepartmentID = model.DepartmentID,
            Name = model.Name
        };
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
        return new DepartmentDetailDto
        {
            DepartmentID = model.DepartmentID,
            Name = model.Name,
            Budget = model.Budget,
            StartDate = model.StartDate,
            AdministratorName = model.AdministratorName,
            InstructorID = model.InstructorID,
            RowVersion = model.RowVersion
        };
    }

    public static DepartmentOverviewDto ToDto(DepartmentOverview model)
    {
        return new DepartmentOverviewDto
        {
            DepartmentID = model.DepartmentID,
            Name = model.Name,
            Budget = model.Budget,
            StartDate = model.StartDate,
            AdministratorName = model.AdministratorName
        };
    }
}
