using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Department
{
    public int DepartmentID { get; set; }
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public int? InstructorID { get; set; }
    public byte[] RowVersion { get; set; }

    public Instructor Administrator { get; set; }
    public ICollection<Course> Courses { get; set; } = new HashSet<Course>();

    public override string ToString()
    {
        return Name;
    }
}
