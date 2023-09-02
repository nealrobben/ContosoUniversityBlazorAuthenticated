namespace WebUI.Shared.Instructors.Commands.CreateInstructor;

using MediatR;
using System;

public class CreateInstructorCommand : IRequest<int>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime HireDate { get; set; }

    public string ProfilePictureName { get; set; }
}
