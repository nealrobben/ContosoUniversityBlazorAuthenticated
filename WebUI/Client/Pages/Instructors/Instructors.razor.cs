
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Client.Extensions;
using WebUI.Client.Services;
using WebUI.Client.ViewModels.Common;
using WebUI.Client.ViewModels.Instructors;

namespace WebUI.Client.Pages.Instructors;

public partial class Instructors
{
    [Inject]
    public IStringLocalizer<Instructors> Localizer { get; set; }

    [Inject]
    public IInstructorService InstructorService { get; set; }

    [Inject]
    public ISnackbar SnackBar { get; set; }

    [Inject]
    public IDialogService DialogService { get; set; }

    private MudTable<InstructorOverviewVM> Table;

    public OverviewVM<InstructorOverviewVM> InstructorsOverview { get; set; } = new OverviewVM<InstructorOverviewVM>();

    public int? SelectedInstructorId { get; set; }
    public int? SelectedCourseId { get; set; }

    protected override void OnInitialized()
    {
        SnackBar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
        SnackBar.Configuration.ClearAfterNavigation = true;
    }

    private async Task GetInstructors()
    {
        await Table.ReloadServerData();
    }

    public async Task DeleteInstructor(int instructorId, string name)
    {
        bool? dialogResult = await DialogService.ShowMessageBox(Localizer["Confirm"], Localizer["DeleteConfirmation", name],
            yesText: Localizer["Delete"], cancelText: Localizer["Cancel"]);

        if (dialogResult == true)
        {
            try
            {
                await InstructorService.DeleteAsync(instructorId.ToString());

                SnackBar.Add(Localizer["DeleteFeedback", name], Severity.Success);
                await GetInstructors();
            }
            catch (System.Exception)
            {
                SnackBar.Add(Localizer["DeleteErrorFeedback", name], Severity.Error);
            }
        }
    }

    public void SelectInstructor(int instructorId)
    {
        SelectedInstructorId = instructorId;
        SelectedCourseId = null;
    }

    public void OnCourseSelected(int courseId)
    {
        SelectedCourseId = courseId;
    }

    public void OpenInstructorDetails(int instructorId)
    {
        var parameters = new DialogParameters
        {
            { "InstructorId", instructorId }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        DialogService.Show<InstructorDetails>(Localizer["InstructorDetails"], parameters, options);
    }

    public async Task OpenInstructorEdit(int instructorId)
    {
        var parameters = new DialogParameters
        {
            { "InstructorId", instructorId }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = DialogService.Show<InstructorEdit>(Localizer["InstructorEdit"], parameters, options);

        var result = await dialog.Result;

        if (result.Data != null && (bool)result.Data)
        {
            await GetInstructors();
        }
    }

    public async Task OpenCreateInstructor()
    {
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = DialogService.Show<InstructorCreate>(Localizer["CreateInstructor"], options);
        var result = await dialog.Result;

        if (result.Data != null && (bool)result.Data)
        {
            await GetInstructors();
        }
    }

    public async Task Filter()
    {
        await GetInstructors();
    }

    public async Task BackToFullList()
    {
        InstructorsOverview.MetaData.SearchString = "";
        await GetInstructors();
    }

    public async Task<TableData<InstructorOverviewVM>> ServerReload(TableState state)
    {
        var searchString = InstructorsOverview?.MetaData.SearchString ?? "";
        var sortString = state.GetSortString();

        var result = await InstructorService.GetAllAsync(sortString, state.Page, searchString, state.PageSize);

        return new TableData<InstructorOverviewVM>() { TotalItems = result.MetaData.TotalRecords, Items = result.Records.Select(x => new InstructorOverviewVM
            {
                InstructorID = x.InstructorID,
                LastName = x.LastName,
                FirstName = x.FirstName,
                HireDate = x.HireDate,
                OfficeLocation = x.OfficeLocation,
                CourseAssignments = x.CourseAssignments.Select(x => new CourseAssignmentVM
                {
                    CourseID = x.CourseID,
                    CourseTitle = x.CourseTitle
                }).ToList()
            }).ToList()
        };
    }

    public string InstructorsSelectRowClassFunc(InstructorOverviewVM instructor, int rowNumber)
    {
        if (instructor?.InstructorID == SelectedInstructorId)
            return "mud-theme-primary";

        return "";
    }
}
