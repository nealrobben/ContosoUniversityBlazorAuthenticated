namespace WebUI.Client.ViewModels.Departments;

public class DepartmentLookupVM
{
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    public override string ToString()
    {
        return $"{DepartmentID} - {Name}";
    }
}
