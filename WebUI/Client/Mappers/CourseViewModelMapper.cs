using System.Linq;
using WebUI.Client.Dtos.Courses;
using WebUI.Client.ViewModels.Courses;

namespace WebUI.Client.Mappers;

public static class CourseViewModelMapper
{
    public static CourseDetailVM ToViewModel(CourseDetailDto dto)
    {
        return new CourseDetailVM
        {
            CourseId = dto.CourseId,
            Title = dto.Title,
            Credits = dto.Credits,
            DepartmentID = dto.DepartmentId
        };
    }

    public static CourseOverviewVM ToViewModel(CourseOverviewDto dto)
    {
        return new CourseOverviewVM
        {
            CourseId = dto.CourseId,
            Title = dto.Title,
            Credits = dto.Credits,
            DepartmentName = dto.DepartmentName
        };
    }

    public static CourseForInstructorVM ToViewModel(CourseForInstructorDto dto)
    {
        return new CourseForInstructorVM
        {
            CourseId = dto.CourseId,
            Title = dto.Title,
            DepartmentName = dto.DepartmentName
        };
    }

    public static CoursesForInstructorOverviewVM ToViewModel(CoursesForInstructorOverviewDto dto)
    {
        return new CoursesForInstructorOverviewVM
        {
            Courses = dto.Courses.Select(ToViewModel).ToList()
        };
    }
}
