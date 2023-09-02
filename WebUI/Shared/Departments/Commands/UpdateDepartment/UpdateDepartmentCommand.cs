namespace WebUI.Shared.Departments.Commands.UpdateDepartment;

using MediatR;
using System;

public class UpdateDepartmentCommand : IRequest
{
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public byte[] RowVersion { get; set; }

    public int InstructorID { get; set; }
}
