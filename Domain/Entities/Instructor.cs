namespace ContosoUniversityBlazor.Domain.Entities;

using System;
using System.Collections.Generic;

public class Instructor : Person
{
    public Instructor()
    {
        CourseAssignments = new HashSet<CourseAssignment>();
        Departments = new HashSet<Department>();
    }

    public DateTime HireDate { get; set; }

    public OfficeAssignment OfficeAssignment { get; set; }
    public ICollection<CourseAssignment> CourseAssignments { get; set; }
    public ICollection<Department> Departments { get; set; }
}
