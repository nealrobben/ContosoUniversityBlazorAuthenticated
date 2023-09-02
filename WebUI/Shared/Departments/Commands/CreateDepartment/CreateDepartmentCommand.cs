namespace WebUI.Shared.Departments.Commands.CreateDepartment;

using MediatR;
using System;

public class CreateDepartmentCommand : IRequest<int>
{
    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public int InstructorID { get; set; }
}
