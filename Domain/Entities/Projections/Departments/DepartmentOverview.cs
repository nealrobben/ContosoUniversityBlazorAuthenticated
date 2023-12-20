using System;

namespace Domain.Entities.Projections.Departments;

public class DepartmentOverview
{
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public string AdministratorName { get; set; }
}
