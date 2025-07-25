﻿
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
using WebUI.Client.ViewModels.Students;

namespace WebUI.Client.Pages.Students;

public partial class Students
{
    [Inject]
    public IStringLocalizer<Students> Localizer { get; set; }

    [Inject]
    public IDialogService DialogService { get; set; }

    [Inject]
    public IStudentService StudentService { get; set; }

    [Inject]
    public ISnackbar Snackbar { get; set; }

    private MudTable<StudentOverviewVM> Table;

    public OverviewVM<StudentOverviewVM> StudentsOverview { get; set; } = new OverviewVM<StudentOverviewVM>();

    protected override void OnInitialized()
    {
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
        Snackbar.Configuration.ClearAfterNavigation = true;
    }

    private async Task GetStudents()
    {
        await Table.ReloadServerData();
    }

    public async Task DeleteStudent(int studentId, string name)
    {
        bool? dialogResult = await DialogService.ShowMessageBox(Localizer["Confirm"], Localizer["DeleteConfirmation", name],
            yesText: Localizer["Delete"], cancelText: Localizer["Cancel"]);

        if (dialogResult == true)
        {
            try
            {
                await StudentService.DeleteAsync(studentId.ToString());

                Snackbar.Add(Localizer["DeleteFeedback", name], Severity.Success);
                await GetStudents();
            }
            catch (System.Exception)
            {
                Snackbar.Add(Localizer["DeleteErrorFeedback", name], Severity.Error);
            }
        }
    }

    public async Task Filter()
    {
        await GetStudents();
    }

    public async Task BackToFullList()
    {
        StudentsOverview.MetaData.SearchString = "";
        await GetStudents();
    }

    public async Task OpenStudentDetails(int studentId)
    {
        var parameters = new DialogParameters
        {
            { "StudentId", studentId }
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Large };

        await DialogService.ShowAsync<StudentDetails>(Localizer["StudentDetails"], parameters, options);
    }

    public async Task OpenStudentEdit(int studentId)
    {
        var parameters = new DialogParameters
        {
            { "StudentId", studentId }
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<StudentEdit>(Localizer["StudentEdit"], parameters, options);

        var result = await dialog.Result;

        if (result.Data != null && (bool)result.Data)
        {
            await GetStudents();
        }
    }

    public async Task OpenStudentCreate()
    {
        var options = new DialogOptions { MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<StudentCreate>(Localizer["CreateStudent"], options);
        var result = await dialog.Result;

        if (result.Data != null && (bool)result.Data)
        {
            await GetStudents();
        }
    }

    public async Task<TableData<StudentOverviewVM>> ServerReload(TableState state, CancellationToken ct)
    {
        var searchString = StudentsOverview?.MetaData.SearchString ?? "";
        var sortString = state.GetSortString();

        var result = await StudentService.GetAllAsync(sortString, state.Page, searchString, state.PageSize, ct);

        return new TableData<StudentOverviewVM>
        {
            TotalItems = result.MetaData.TotalRecords,
            Items = result.Records.Select(StudentViewModelMapper.ToViewModel)
        };
    }
}
