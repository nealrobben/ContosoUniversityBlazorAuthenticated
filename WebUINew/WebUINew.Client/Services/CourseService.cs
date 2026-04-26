using System.Net.Http.Json;
using WebUINew.Client.Dtos.Common;
using WebUINew.Client.Dtos.Courses;

namespace WebUINew.Client.Services;

public interface ICourseService
    : IServiceBase<OverviewDto<CourseOverviewDto>, CourseDetailDto,
        CreateCourseDto, UpdateCourseDto>
{
    Task<CoursesForInstructorOverviewDto> GetCoursesForInstructor(string instructorId);
}

public class CourseService
    : ServiceBase<OverviewDto<CourseOverviewDto>, CourseDetailDto,
        CreateCourseDto, UpdateCourseDto>, ICourseService
{
    public CourseService(HttpClient http)
        : base(http, "courses")
    {
    }

    public async Task<CoursesForInstructorOverviewDto> GetCoursesForInstructor(string instructorId)
    {
        return await _http.GetFromJsonAsync<CoursesForInstructorOverviewDto>($"{Endpoint}/byinstructor/{instructorId}");
    }
}