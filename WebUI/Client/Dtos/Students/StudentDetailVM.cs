using System;
using System.Collections.Generic;

namespace WebUI.Client.Dtos.Students;

public class StudentDetailDto
{
    public int StudentID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }

    public List<StudentDetailEnrollmentDto> Enrollments { get; set; }
}
