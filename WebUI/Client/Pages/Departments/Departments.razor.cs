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
using WebUI.Client.ViewModels.Departments;

namespace WebUI.Client.Pages.Departments;

public partial class Departments
{
    [Inject]
    public IDialogService DialogService { get; set; }

    [Inject]
    public ISnackbar SnackBar { get; set; }

    [Inject]
    public IDepartmentService DepartmentService { get; set; }

    [Inject]
    public IStringLocalizer<Departments> Localizer { get; set; }

    private MudTable<DepartmentOverviewVM> Table;

    public OverviewVM<DepartmentOverviewVM> DepartmentsOverview { get; set; } = new OverviewVM<DepartmentOverviewVM>();

    protected override void OnInitialized()
    {
        SnackBar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
        SnackBar.Configuration.ClearAfterNavigation = true;
    }

    private async Task GetDepartments()
    {
        await Table.ReloadServerData();
    }

    public async Task DeleteDepartment(int departmentId, string departmentName)
    {
        bool? dialogResult = await DialogService.ShowMessageBox(Localizer["Confirm"], Localizer["DeleteConfirmation", departmentName],
            yesText: Localizer["Delete"], cancelText: Localizer["Cancel"]);

        if (dialogResult == true)
        {
            try
            {
                await DepartmentService.DeleteAsync(departmentId.ToString());

                SnackBar.Add(Localizer["DeleteFeedback", departmentName], Severity.Success);
                await GetDepartments();
            }
            catch (System.Exception)
            {
                SnackBar.Add(Localizer["DeleteErrorFeedback", departmentName], Severity.Error);
            }
        }
    }

    public async Task OpenDepartmentDetails(int departmentId)
    {
        var parameters = new DialogParameters
        {
            { "DepartmentId", departmentId }
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.ExtraSmall };

        await DialogService.ShowAsync<DepartmentDetails>(Localizer["DepartmentDetails"], parameters, options);
    }

    public async Task OpenDepartmentEdit(int departmentId)
    {
        var parameters = new DialogParameters
        {
            { "DepartmentId", departmentId }
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DepartmentEdit>(Localizer["DepartmentEdit"], parameters, options);

        var result = await dialog.Result;

        if (result.Data != null && (bool)result.Data)
        {
            await GetDepartments();
        }
    }

    public async Task OpenDepartmentCreate()
    {
        var options = new DialogOptions { MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<DepartmentCreate>(Localizer["CreateDepartment"], options);
        var result = await dialog.Result;

        if (result.Data != null && (bool)result.Data)
        {
            await GetDepartments();
        }
    }

    public async Task Filter()
    {
        await GetDepartments();
    }

    public async Task BackToFullList()
    {
        DepartmentsOverview.MetaData.SearchString = "";
        await GetDepartments();
    }

    public async Task<TableData<DepartmentOverviewVM>> ServerReload(TableState state, CancellationToken ct)
    {
        var searchString = DepartmentsOverview?.MetaData.SearchString ?? "";
        var sortString = state.GetSortString();

        var result = await DepartmentService.GetAllAsync(sortString, state.Page, searchString, state.PageSize, ct);

        return new TableData<DepartmentOverviewVM>
        {
            TotalItems = result.MetaData.TotalRecords,
            Items = result.Records.Select(DepartmentViewModelMapper.ToViewModel)
        };
    }
}
