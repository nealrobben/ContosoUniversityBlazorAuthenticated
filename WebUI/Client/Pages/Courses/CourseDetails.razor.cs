
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Services;
using WebUI.Client.ViewModels.Courses;

namespace WebUI.Client.Pages.Courses;

public partial class CourseDetails
{
    [Inject]
    public IStringLocalizer<CourseDetails> Localizer { get; set; }

    [Inject]
    public ICourseService CourseService { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public int CourseId { get; set; }

    public CourseDetailVM Course { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var courseDto = await CourseService.GetAsync(CourseId.ToString());

        Course = new CourseDetailVM
        {
            CourseID = courseDto.CourseID,
            Title = courseDto.Title,
            Credits = courseDto.Credits,
            DepartmentID = courseDto.DepartmentID
        };
    }

    public void Close()
    {
        MudDialog.Cancel();
    }
}
