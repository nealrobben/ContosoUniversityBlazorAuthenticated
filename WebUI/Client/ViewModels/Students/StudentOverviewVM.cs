using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Client.ViewModels.Students;

public class StudentOverviewVM
{
    public int StudentID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime EnrollmentDate { get; set; }

    public override string ToString()
    {
        return FullName;
    }
}
