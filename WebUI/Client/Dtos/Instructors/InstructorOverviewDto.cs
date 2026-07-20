using System;
using System.Collections.Generic;

namespace WebUI.Client.Dtos.Instructors;

public record InstructorOverviewDto(int InstructorId, string LastName, string FirstName, DateTime HireDate, string OfficeLocation, List<CourseAssignmentDto> CourseAssignments)
{
    public string FullName => $"{FirstName} {LastName}";
}