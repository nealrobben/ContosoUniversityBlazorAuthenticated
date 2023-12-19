using System;

namespace WebUI.Client.Dtos.Departments;

public class DepartmentDetailDto
{
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public string AdministratorName { get; set; }

    public int? InstructorID { get; set; }

    public byte[] RowVersion { get; set; }
}
