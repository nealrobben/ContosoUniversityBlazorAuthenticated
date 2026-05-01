using System.Collections.Generic;

namespace WebUI.Client.Dtos.Departments;

public record DepartmentsLookupDto(IList<DepartmentLookupDto> Departments)
{
    public DepartmentsLookupDto() : this([])
    {
    }
}