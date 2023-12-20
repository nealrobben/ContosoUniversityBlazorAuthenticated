
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Courses;
using WebUI.Shared.Common;
using WebUI.Shared.Courses.Queries.GetCoursesOverview;

namespace WebUI.Client.Services;

public interface ICourseService 
    : IServiceBase<OverviewVM<CourseVM>, CourseDetailDto,
        CreateCourseDto, UpdateCourseDto>
{
    Task<CoursesForInstructorOverviewDto> GetCoursesForInstructor(string instructorId);
}

public class CourseService 
    : ServiceBase<OverviewVM<CourseVM>, CourseDetailDto,
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
