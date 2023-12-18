
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Courses;
using WebUI.Shared.Common;
using WebUI.Shared.Courses.Queries.GetCourseDetails;
using WebUI.Shared.Courses.Queries.GetCoursesForInstructor;
using WebUI.Shared.Courses.Queries.GetCoursesOverview;

namespace WebUI.Client.Services;

public interface ICourseService 
    : IServiceBase<OverviewVM<CourseVM>, CourseDetailVM,
        CreateCourseDto, UpdateCourseDto>
{
    Task<CoursesForInstructorOverviewVM> GetCoursesForInstructor(string instructorId);
}

public class CourseService 
    : ServiceBase<OverviewVM<CourseVM>, CourseDetailVM,
        CreateCourseDto, UpdateCourseDto>, ICourseService
{
    public CourseService(HttpClient http) 
        : base(http, "courses")
    {
    }

    public async Task<CoursesForInstructorOverviewVM> GetCoursesForInstructor(string instructorId)
    {
        return await _http.GetFromJsonAsync<CoursesForInstructorOverviewVM>($"{Endpoint}/byinstructor/{instructorId}");
    }
}
