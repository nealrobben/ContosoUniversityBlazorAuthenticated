using System;

namespace WebUI.Client.ViewModels.Instructors;

public class InstructorDetailVM
{
    public int InstructorID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime HireDate { get; set; }

    public string OfficeLocation { get; set; }

    public string ProfilePictureName { get; set; }

    public override string ToString()
    {
        return $"{InstructorID} - {LastName} - {FirstName}";
    }
}
