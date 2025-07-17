
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebUI.Client.Extensions;
using WebUI.Client.Mappers;
using WebUI.Client.Services;
using WebUI.Client.ViewModels.Common;
using WebUI.Client.ViewModels.Courses;

namespace WebUI.Client.Pages.Courses;

public partial class Courses
{
    [Inject]
    public IDialogService DialogService { get; set; }

    [Inject]
    public ISnackbar Snackbar { get; set; }

    [Inject]
    public ICourseService CourseService { get; set; }

    [Inject]
    public IStringLocalizer<Courses> Localizer { get; set; }

    public MudTable<CourseOverviewVM> Table { get; set; }

    public OverviewVM<CourseOverviewVM> CoursesOverview { get; set; } = new OverviewVM<CourseOverviewVM>();

    protected override void OnInitialized()
    {
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
        Snackbar.Configuration.ClearAfterNavigation = true;
    }

    private async Task GetCourses()
    {
        await Table.ReloadServerData();
    }

    public async Task DeleteCourse(int courseId, string title)
    {
        bool? dialogResult = await DialogService.ShowMessageBox(Localizer["Confirm"], Localizer["DeleteConfirmation", title],
            yesText: Localizer["Delete"], cancelText: Localizer["Cancel"]);

        if (dialogResult == true)
        {
            try
            {
                await CourseService.DeleteAsync(courseId.ToString());

                Snackbar.Add(Localizer["DeleteFeedback", title], Severity.Success);
                await GetCourses();
            }
            catch (System.Exception)
            {
                Snackbar.Add(Localizer["DeleteErrorFeedback", title], Severity.Error);
            }
        }
    }

    public async Task OpenCourseDetails(int courseId)
    {
        var parameters = new DialogParameters
        {
            { "CourseId", courseId }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };

        await DialogService.ShowAsync<CourseDetails>(Localizer["CourseDetails"], parameters, options);
    }

    public async Task OpenCourseEdit(int courseId)
    {
        var parameters = new DialogParameters
        {
            { "CourseId", courseId }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<CourseEdit>(Localizer["CourseEdit"], parameters, options);

        var result = await dialog.Result;

        if (result.Data != null && (bool)result.Data)
        {
            await GetCourses();
        }
    }

    public async Task OpenCourseCreate()
    {
        var options = new DialogOptions() { MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<CourseCreate>(Localizer["CreateCourse"], options);
        var result = await dialog.Result;

        if (result.Data != null && (bool)result.Data)
        {
            await GetCourses();
        }
    }

    public async Task Filter()
    {
        await GetCourses();
    }

    public async Task BackToFullList()
    {
        CoursesOverview.MetaData.SearchString = "";
        await GetCourses();
    }

    public async Task<TableData<CourseOverviewVM>> ServerReload(TableState state, CancellationToken ct)
    {
        var searchString = CoursesOverview?.MetaData.SearchString ?? "";
        var sortString = state.GetSortString();

        var result = await CourseService.GetAllAsync(sortString, state.Page, searchString, state.PageSize);

        return new TableData<CourseOverviewVM>
        {
            TotalItems = result.MetaData.TotalRecords,
            Items = result.Records.Select(CourseViewModelMapper.ToViewModel).ToList()
        };
    }
}
