
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Shared.Students.Commands.UpdateStudent;
using WebUI.Shared.Students.Queries.GetStudentDetails;
using WebUI.Shared.Students.Queries.GetStudentsOverview;
using WebUI.Shared.Students.Queries.GetStudentsForCourse;
using WebUI.Shared.Common;
using WebUI.Client.Dtos.Students;

namespace WebUI.Client.Services;

public interface IStudentService 
    : IServiceBase<OverviewVM<StudentOverviewVM>, StudentDetailsVM, 
        CreateStudentDto, UpdateStudentCommand>
{
    Task<StudentsForCourseVM> GetStudentsForCourse(string courseId);
}

public class StudentService 
    : ServiceBase<OverviewVM<StudentOverviewVM>, StudentDetailsVM,
        CreateStudentDto, UpdateStudentCommand>, IStudentService
{
    public StudentService(HttpClient http) 
        : base(http, "students")
    {
    }

    public async Task<StudentsForCourseVM> GetStudentsForCourse(string courseId)
    {
        return await _http.GetFromJsonAsync<StudentsForCourseVM>($"{Endpoint}/bycourse/{courseId}");
    }
}
