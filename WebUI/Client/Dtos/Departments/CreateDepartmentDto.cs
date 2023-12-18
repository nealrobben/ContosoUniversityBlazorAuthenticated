using System;

namespace WebUI.Client.Dtos.Departments;

public class CreateDepartmentDto
{
    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public int InstructorID { get; set; }
}
