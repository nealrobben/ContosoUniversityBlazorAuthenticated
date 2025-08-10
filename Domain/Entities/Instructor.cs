using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Instructor : Person
{
    public DateTime HireDate { get; set; }

    public OfficeAssignment OfficeAssignment { get; set; }
    public ICollection<CourseAssignment> CourseAssignments { get; set; } = new HashSet<CourseAssignment>();
    public ICollection<Department> Departments { get; set; } = new HashSet<Department>();
}
