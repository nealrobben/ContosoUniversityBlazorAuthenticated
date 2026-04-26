namespace WebUINew.Client.ViewModels.Departments;

public class DepartmentLookupVM
{
    public int DepartmentId { get; set; }

    public string Name { get; set; }

    public override string ToString()
    {
        return $"{DepartmentId} - {Name}";
    }
}