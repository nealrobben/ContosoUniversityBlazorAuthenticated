using System;

namespace WebUI.Client.InputModels.Students;

public class CreateStudentInputModel
{
    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }
}
