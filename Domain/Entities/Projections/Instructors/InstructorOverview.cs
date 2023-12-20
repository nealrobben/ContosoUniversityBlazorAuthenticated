using System;
using System.Collections.Generic;

namespace Domain.Entities.Projections.Instructors;

public class InstructorOverview
{
    public int InstructorID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public DateTime HireDate { get; set; }

    public string OfficeLocation { get; set; }

    public List<CourseAssignment> CourseAssignments { get; set; }
}
