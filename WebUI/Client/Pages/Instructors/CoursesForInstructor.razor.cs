
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Client.Mappers;
using WebUI.Client.Services;
using WebUI.Client.ViewModels.Courses;

namespace WebUI.Client.Pages.Instructors;

public partial class CoursesForInstructor
{
    [Inject]
    public IStringLocalizer<Instructors> Localizer { get; set; }

    [Inject]
    public ICourseService CourseService { get; set; }

    [Parameter]
    public int? SelectedInstructorId { get; set; }

    [Parameter]
    public int? SelectedCourseId { get; set; }

    [Parameter] 
    public EventCallback<int> OnCourseSelected { get; set; }

    public CoursesForInstructorOverviewVM CourseForInstructorOverview { get; set; } = new CoursesForInstructorOverviewVM();

    protected override async Task OnParametersSetAsync()
    {
        if(SelectedInstructorId != null)
        {
            var courseForInstructorOverview = await CourseService.GetCoursesForInstructor(SelectedInstructorId.ToString());
            CourseForInstructorOverview = CourseViewModelMapper.ToViewModel(courseForInstructorOverview);
        }
    }

    public string CoursesSelectRowClassFunc(CourseForInstructorVM course, int rowNumber)
    {
        if (course?.CourseID == SelectedCourseId)
            return "mud-theme-primary";

        return "";
    }
}
