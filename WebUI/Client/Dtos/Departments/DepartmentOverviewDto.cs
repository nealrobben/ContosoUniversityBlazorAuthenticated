using System;

namespace WebUI.Client.Dtos.Departments;

public class DepartmentOverviewDto
{
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public string AdministratorName { get; set; }
}
