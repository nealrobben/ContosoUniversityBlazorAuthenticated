using Domain.Entities.Projections.Courses;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.Projections.Mappers;

public static class CourseProjectionMapper
{
    public static CourseDetail ToCourseDetailProjection(Course course)
    {
        return new CourseDetail
        {
            CourseID = course.CourseID,
            Title = course.Title,
            Credits = course.Credits,
            DepartmentID = course.DepartmentID
        };
    }

    public static CourseForInstructor ToCourseForInstructorProjection(Course course)
    {
        return new CourseForInstructor
        {
            CourseID = course.CourseID,
            Title = course.Title,
            DepartmentName = course.Department?.Name ?? string.Empty
        };
    }

    public static CoursesForInstructorOverview ToCoursesForInstructorProjection(IEnumerable<Course> courses)
    {
        return new CoursesForInstructorOverview
        {
            Courses = courses.Select(ToCourseForInstructorProjection).ToList()
        };
    }

    public static CourseOverview ToCourseOverviewProjection(Course course)
    {
        return new CourseOverview
        {
            CourseID = course.CourseID,
            Title = course.Title,
            Credits = course.Credits,
            DepartmentName = course.Department?.Name ?? string.Empty
        };
    }
}
