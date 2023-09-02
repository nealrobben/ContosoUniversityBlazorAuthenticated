namespace WebUI.Shared.Students.Commands.UpdateStudent;

using MediatR;
using System;

public class UpdateStudentCommand : IRequest
{
    public int? StudentID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }
}
