using System.Collections.Generic;

namespace WebUI.Client.ViewModels.Departments;

public class DepartmentsLookupVM
{
    public IList<DepartmentLookupVM> Departments { get; set; }

    public DepartmentsLookupVM()
    {
        Departments = new List<DepartmentLookupVM>();
    }

    public DepartmentsLookupVM(IList<DepartmentLookupVM> departments)
    {
        Departments = departments;
    }
}
