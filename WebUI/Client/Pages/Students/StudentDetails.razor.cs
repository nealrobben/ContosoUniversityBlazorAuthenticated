
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Client.Enums;
using WebUI.Client.Services;
using WebUI.Client.ViewModels.Students;

namespace WebUI.Client.Pages.Students;

public partial class StudentDetails
{
    [Inject]
    public IStudentService StudentService { get; set; }

    [Inject]
    IStringLocalizer<StudentDetails> Localizer { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public int StudentId { get; set; }

    public StudentDetailVM Student { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var student = await StudentService.GetAsync(StudentId.ToString());

        Student = new StudentDetailVM
        {
            StudentID = student.StudentID,
            LastName = student.LastName,
            FirstName = student.FirstName,
            EnrollmentDate = student.EnrollmentDate,
            ProfilePictureName = student.ProfilePictureName,
            Enrollments = student.Enrollments.Select(x => new StudentDetailEnrollmentVM
            {
                CourseTitle = x.CourseTitle,
                Grade = (Grade?)x.Grade,
            }).ToList()
        };
    }

    public void Close()
    {
        MudDialog.Cancel();
    }
}
