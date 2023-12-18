using System;

namespace WebUI.Client.Dtos.Students;

public class UpdateStudentDto
{
    public int? StudentID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }
}
