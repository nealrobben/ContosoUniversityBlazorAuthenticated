using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Client.ViewModels.Students;

public class StudentDetailVM
{
    public int StudentID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }

    public List<StudentDetailEnrollmentVM> Enrollments { get; set; } = [];
}
