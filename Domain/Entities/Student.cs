using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Student : Person
{
    public DateTime EnrollmentDate { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
}
