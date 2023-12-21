using System.Linq;
using WebUI.Client.Dtos.Departments;
using WebUI.Client.ViewModels.Departments;

namespace WebUI.Client.Mappers;

public static class DepartmentViewModelMapper
{
    public static DepartmentDetailVM ToViewModel(DepartmentDetailDto dto)
    {
        return new DepartmentDetailVM
        {
            DepartmentID = dto.DepartmentID,
            Name = dto.Name,
            Budget = dto.Budget,
            StartDate = dto.StartDate,
            AdministratorName = dto.AdministratorName,
            InstructorID = dto.InstructorID,
            RowVersion = dto.RowVersion
        };
    }

    public static DepartmentOverviewVM ToViewModel(DepartmentOverviewDto dto)
    {
        return new DepartmentOverviewVM
        {
            DepartmentID = dto.DepartmentID,
            Name = dto.Name,
            Budget = dto.Budget,
            StartDate = dto.StartDate,
            AdministratorName = dto.AdministratorName
        };
    }

    public static DepartmentLookupVM ToViewModel(DepartmentLookupDto dto)
    {
        return new DepartmentLookupVM
        {
            DepartmentID = dto.DepartmentID,
            Name = dto.Name
        };
    }

    public static DepartmentsLookupVM ToViewModel(DepartmentsLookupDto dto)
    {
        return new DepartmentsLookupVM
        {
            Departments = dto.Departments.Select(ToViewModel).ToList()
        };
    }
}
