using System;
using System.Collections.Generic;

namespace WebUI.Client.ViewModels.Instructors;

public class InstructorOverviewVM
{
    public int InstructorID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public DateTime HireDate { get; set; }

    public string OfficeLocation { get; set; }

    public List<CourseAssignmentVM> CourseAssignments { get; set; }

    public override string ToString()
    {
        return $"{InstructorID} - {LastName} - {FirstName}";
    }
}
