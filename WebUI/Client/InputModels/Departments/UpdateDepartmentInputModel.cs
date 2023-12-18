using System;

namespace WebUI.Client.InputModels.Departments;

public class UpdateDepartmentInputModel
{
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public byte[] RowVersion { get; set; }

    public int InstructorID { get; set; }
}
