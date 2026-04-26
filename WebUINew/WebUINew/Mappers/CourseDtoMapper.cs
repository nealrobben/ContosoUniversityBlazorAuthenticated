using Domain.Entities.Projections.Courses;
using WebUINew.Client.Dtos.Courses;

namespace WebUINew.Mappers;

public static class CourseDtoMapper
{
    public static CourseForInstructorDto ToDto(CourseForInstructor model)
    {
        return new CourseForInstructorDto
        {
            CourseId = model.CourseID,
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
            CourseId = model.CourseID,
            Title = model.Title,
            Credits = model.Credits,
            DepartmentId = model.DepartmentID
        };
    }

    public static CourseOverviewDto ToDto(CourseOverview model)
    {
        return new CourseOverviewDto
        {
            CourseId = model.CourseID,
            Title = model.Title,
            Credits = model.Credits,
            DepartmentName = model.DepartmentName
        };
    }
}