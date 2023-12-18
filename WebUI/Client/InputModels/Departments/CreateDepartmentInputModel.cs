using System;

namespace WebUI.Client.InputModels.Departments;

public class CreateDepartmentInputModel
{
    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public int InstructorID { get; set; }
}
