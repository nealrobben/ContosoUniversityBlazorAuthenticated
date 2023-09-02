namespace WebUI.Shared.Departments.Queries.GetDepartmentsLookup;

using AutoMapper;
using ContosoUniversityBlazor.Domain.Entities;
using WebUI.Shared.Common.Mappings;

public class DepartmentLookupVM : IMapFrom<Department>
{
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Department, DepartmentLookupVM>();
    }

    public override string ToString()
    {
        return $"{DepartmentID} - {Name}";
    }
}
