namespace WebUI.Shared.Instructors.Commands.UpdateInstructor;

using MediatR;
using System;

public class UpdateInstructorCommand : IRequest
{
    public int? InstructorID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime HireDate { get; set; }

    public string OfficeLocation { get; set; }

    public string ProfilePictureName { get; set; }
}
