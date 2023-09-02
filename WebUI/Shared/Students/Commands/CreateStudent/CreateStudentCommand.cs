namespace WebUI.Shared.Students.Commands.CreateStudent;

using MediatR;
using System;

public class CreateStudentCommand : IRequest<int>
{
    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }
}
