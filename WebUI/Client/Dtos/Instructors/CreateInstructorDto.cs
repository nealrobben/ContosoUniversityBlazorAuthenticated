using System;

namespace WebUI.Client.Dtos.Instructors;

public class CreateInstructorDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime HireDate { get; set; }

    public string ProfilePictureName { get; set; }
}
