using System;

namespace WebUI.Client.InputModels.Instructors;

public class CreateInstructorInputModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime HireDate { get; set; }

    public string ProfilePictureName { get; set; }
}
