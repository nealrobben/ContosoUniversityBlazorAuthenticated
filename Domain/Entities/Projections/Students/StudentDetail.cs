using System;
using System.Collections.Generic;

namespace Domain.Entities.Projections.Students;

public class StudentDetail
{
    public int StudentID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }

    public List<StudentDetailEnrollment> Enrollments { get; set; }
}
