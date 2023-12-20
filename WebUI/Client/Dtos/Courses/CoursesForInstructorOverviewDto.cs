using System.Collections.Generic;

namespace WebUI.Client.Dtos.Courses;

public class CoursesForInstructorOverviewDto
{
    public IList<CourseForInstructorDto> Courses { get; set; }
}
