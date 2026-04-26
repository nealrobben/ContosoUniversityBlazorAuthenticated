using WebUINew.Client.Dtos.Departments;
using WebUINew.Client.ViewModels.Departments;

namespace WebUINew.Client.Mappers;

public static class DepartmentViewModelMapper
{
    public static DepartmentDetailVM ToViewModel(DepartmentDetailDto dto)
    {
        return new DepartmentDetailVM
        {
            DepartmentId = dto.DepartmentId,
            Name = dto.Name,
            Budget = dto.Budget,
            StartDate = dto.StartDate,
            AdministratorName = dto.AdministratorName,
            InstructorId = dto.InstructorId,
            RowVersion = dto.RowVersion
        };
    }

    public static DepartmentOverviewVM ToViewModel(DepartmentOverviewDto dto)
    {
        return new DepartmentOverviewVM
        {
            DepartmentId = dto.DepartmentID,
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
            DepartmentId = dto.DepartmentID,
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