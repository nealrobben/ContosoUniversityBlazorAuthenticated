
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Client.Enums;
using WebUI.Client.Services;
using WebUI.Client.ViewModels.Students;

namespace WebUI.Client.Pages.Instructors;

public partial class StudentsForCourse
{
    [Inject]
    public IStringLocalizer<Instructors> Localizer { get; set; }

    [Inject]
    public IStudentService StudentService { get; set; }

    [Parameter]
    public int? SelectedCourseId { get; set; }

    public StudentsForCourseVM StudentsForCourseVM { get; set; } = new StudentsForCourseVM();

    protected override async Task OnParametersSetAsync()
    {
        if(SelectedCourseId != null)
        {
            var studentsForCourseVM = await StudentService.GetStudentsForCourse(SelectedCourseId.ToString());

            StudentsForCourseVM = new StudentsForCourseVM
            {
                Students = studentsForCourseVM.Students.Select(x => new StudentForCourseVM
                {
                    StudentName = x.StudentName,
                    StudentGrade = (Grade?)x.StudentGrade
                }).ToList()
            };
        }
    }
}
