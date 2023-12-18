using System;

namespace WebUI.Client.InputModels.Students;

public class UpdateStudentInputModel
{
    public int? StudentID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }
}
