using Domain.Entities.Projections.Courses;
using System.Linq;
using WebUI.Client.Dtos.Courses;

namespace WebUI.Server.Mappers;

public static class CourseDtoMapper
{
    public static CourseForInstructorDto ToDto(CourseForInstructor model)
    {
        return new CourseForInstructorDto
        {
            CourseID = model.CourseID,
            Title = model.Title,
            DepartmentName = model.DepartmentName,
        };
    }

    public static CoursesForInstructorOverviewDto ToDto(CoursesForInstructorOverview model)
    {
        return new CoursesForInstructorOverviewDto
        {
            Courses = model.Courses.Select(ToDto).ToList()
        };
    }

    public static CourseDetailDto ToDto(CourseDetail model)
    {
        return new CourseDetailDto
        {
            CourseID = model.CourseID,
            Title = model.Title,
            Credits = model.Credits,
            DepartmentID = model.DepartmentID
        };
    }

    public static CourseOverviewDto ToDto(CourseOverview model)
    {
        return new CourseOverviewDto
        {
            CourseID = model.CourseID,
            Title = model.Title,
            Credits = model.Credits,
            DepartmentName = model.DepartmentName
        };
    }
}
