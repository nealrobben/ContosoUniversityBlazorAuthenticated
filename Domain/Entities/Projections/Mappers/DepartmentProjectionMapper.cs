
using ContosoUniversityBlazor.Domain.Entities;
using Domain.Entities.Projections.Departments;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.Projections.Mappers;

public static class DepartmentProjectionMapper
{
    public static DepartmentDetail ToDepartmentDetailProjection(Department model)
    {
        return new DepartmentDetail
        {
            DepartmentID = model.DepartmentID,
            Name = model.Name,
            Budget = model.Budget,
            StartDate = model.StartDate,
            AdministratorName = model.Administrator?.FullName ?? string.Empty,
            InstructorID = model.InstructorID,
            RowVersion = model.RowVersion
        };
    }

    public static DepartmentLookup ToDepartmentLookupProjection(Department model)
    {
        return new DepartmentLookup
        {
            DepartmentID = model.DepartmentID,
            Name = model.Name
        };
    }

    public static DepartmentsLookup ToDepartmentsLookupProjection(IEnumerable<Department> departments)
    {
        return new DepartmentsLookup
        {
            Departments = departments.Select(ToDepartmentLookupProjection).ToList()
        };
    }

    public static DepartmentOverview ToDepartmentOverviewProjection(Department department)
    {
        return new DepartmentOverview
        {
            DepartmentID = department.DepartmentID,
            Name = department.Name,
            Budget = department.Budget,
            StartDate = department.StartDate,
            AdministratorName = department.Administrator?.FullName ?? string.Empty
        };
    }
}
