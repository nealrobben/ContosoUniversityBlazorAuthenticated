namespace WebUINew.Client.ViewModels.Departments;

public class DepartmentsLookupVM
{
    public IList<DepartmentLookupVM> Departments { get; set; }

    public DepartmentsLookupVM()
    {
        Departments = [];
    }

    public DepartmentsLookupVM(IList<DepartmentLookupVM> departments)
    {
        Departments = departments;
    }
}