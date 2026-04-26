namespace WebUINew.Client.Dtos.Departments;

public class DepartmentsLookupDto
{
    public IList<DepartmentLookupDto> Departments { get; set; }

    public DepartmentsLookupDto()
    {
        Departments = [];
    }

    public DepartmentsLookupDto(IList<DepartmentLookupDto> departments)
    {
        Departments = departments;
    }
}