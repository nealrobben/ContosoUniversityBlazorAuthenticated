using System;

namespace WebUI.Client.Dtos.Instructors;

public class UpdateInstructorDto
{
    public int? InstructorID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime HireDate { get; set; }

    public string OfficeLocation { get; set; }

    public string ProfilePictureName { get; set; }
}
