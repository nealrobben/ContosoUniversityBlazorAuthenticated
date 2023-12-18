using System;

namespace WebUI.Client.Dtos.Students;

public class CreateStudentDto
{
    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }
}
