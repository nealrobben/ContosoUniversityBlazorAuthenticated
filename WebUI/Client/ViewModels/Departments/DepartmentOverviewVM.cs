using System;

namespace WebUI.Client.ViewModels.Departments;

public class DepartmentOverviewVM
{
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public string AdministratorName { get; set; }

    public override string ToString()
    {
        return $"{DepartmentID} - {Name}";
    }
}
