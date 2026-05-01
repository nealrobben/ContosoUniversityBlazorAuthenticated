using System.Linq;
using Domain.Entities.Projections.Courses;
using WebUI.Client.Dtos.Courses;

namespace WebUI.Server.Mappers;

public static class CourseDtoMapper
{
    public static CourseForInstructorDto ToDto(CourseForInstructor model)
    {
        return new CourseForInstructorDto(model.CourseID, model.Title, model.DepartmentName);
    }

    public static CoursesForInstructorOverviewDto ToDto(CoursesForInstructorOverview model)
    {
        return new CoursesForInstructorOverviewDto(model.Courses.Select(ToDto).ToList());
    }

    public static CourseDetailDto ToDto(CourseDetail model)
    {
        return new CourseDetailDto(model.CourseID, model.Title, model.Credits, model.DepartmentID);
    }

    public static CourseOverviewDto ToDto(CourseOverview model)
    {
        return new CourseOverviewDto(model.CourseID, model.Title, model.Credits, model.DepartmentName);
    }
}
