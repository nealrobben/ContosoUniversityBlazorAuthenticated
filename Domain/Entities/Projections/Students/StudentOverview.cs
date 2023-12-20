using System;

namespace Domain.Entities.Projections.Students;

public class StudentOverview
{
    public int StudentID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime EnrollmentDate { get; set; }
}
