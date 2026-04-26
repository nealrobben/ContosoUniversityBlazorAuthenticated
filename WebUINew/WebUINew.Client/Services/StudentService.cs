using System.Net.Http.Json;
using WebUINew.Client.Dtos.Common;
using WebUINew.Client.Dtos.Students;

namespace WebUINew.Client.Services;

public interface IStudentService
    : IServiceBase<OverviewDto<StudentOverviewDto>, StudentDetailDto,
        CreateStudentDto, UpdateStudentDto>
{
    Task<StudentsForCourseDto> GetStudentsForCourse(string courseId);
}

public class StudentService
    : ServiceBase<OverviewDto<StudentOverviewDto>, StudentDetailDto,
        CreateStudentDto, UpdateStudentDto>, IStudentService
{
    public StudentService(HttpClient http)
        : base(http, "students")
    {
    }

    public async Task<StudentsForCourseDto> GetStudentsForCourse(string courseId)
    {
        return await _http.GetFromJsonAsync<StudentsForCourseDto>($"{Endpoint}/bycourse/{courseId}");
    }
}