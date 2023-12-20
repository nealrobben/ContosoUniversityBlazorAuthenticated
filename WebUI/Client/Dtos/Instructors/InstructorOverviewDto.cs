using System;
using System.Collections.Generic;

namespace WebUI.Client.Dtos.Instructors;

public class InstructorOverviewDto
{
    public int InstructorID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public DateTime HireDate { get; set; }

    public string OfficeLocation { get; set; }

    public List<CourseAssignmentDto> CourseAssignments { get; set; }
}
