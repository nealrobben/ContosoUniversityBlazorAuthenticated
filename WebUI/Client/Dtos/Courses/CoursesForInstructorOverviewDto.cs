using System.Collections.Generic;

namespace WebUI.Client.Dtos.Courses;

public record CoursesForInstructorOverviewDto(IList<CourseForInstructorDto> Courses);
