namespace ContosoUniversityBlazor.Domain.Entities;

using System;
using System.Collections.Generic;

public class Student : Person
{
    public Student()
    {
        Enrollments = new HashSet<Enrollment>();
    }

    public DateTime EnrollmentDate { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
}
