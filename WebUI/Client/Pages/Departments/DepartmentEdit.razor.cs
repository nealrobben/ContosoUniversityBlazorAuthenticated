
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Departments;
using WebUI.Client.Extensions;
using WebUI.Client.InputModels.Departments;
using WebUI.Client.Mappers;
using WebUI.Client.Services;
using WebUI.Client.Shared;
using WebUI.Client.ViewModels.Instructors;

namespace WebUI.Client.Pages.Departments;

public partial class DepartmentEdit
{
    [Parameter]
    public int DepartmentId { get; set; }

    [Inject]
    public IDepartmentService DepartmentService { get; set; }

    [Inject]
    IStringLocalizer<DepartmentEdit> Localizer { get; set; }

    [Inject]
    public IInstructorService InstructorService { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    private CustomValidation _customValidation;

    public bool ErrorVisible { get; set; }

    public UpdateDepartmentInputModel UpdateDepartmentInputModel { get; set; } = new UpdateDepartmentInputModel();
    public InstructorsLookupVM InstructorsLookup { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var instructorsLookup = await InstructorService.GetLookupAsync();
        InstructorsLookup = InstructorViewModelMapper.ToViewModel(instructorsLookup);

        var department = await DepartmentService.GetAsync(DepartmentId.ToString());

        UpdateDepartmentInputModel.DepartmentID = department.DepartmentID;
        UpdateDepartmentInputModel.Budget = department.Budget;
        UpdateDepartmentInputModel.InstructorID = department.InstructorID ?? 0;
        UpdateDepartmentInputModel.Name = department.Name;
        UpdateDepartmentInputModel.StartDate = department.StartDate;
        UpdateDepartmentInputModel.RowVersion = department.RowVersion;
    }

    public async Task FormSubmitted(EditContext editContext)
    {
        _customValidation.ClearErrors();
        ErrorVisible = false;
        bool formIsValid = editContext.Validate();

        if (formIsValid)
        {
            try
            {
                await DepartmentService.UpdateAsync(new UpdateDepartmentDto
                {
                    DepartmentID = UpdateDepartmentInputModel.DepartmentID,
                    Name = UpdateDepartmentInputModel.Name,
                    Budget = UpdateDepartmentInputModel.Budget,
                    StartDate = UpdateDepartmentInputModel.StartDate,
                    RowVersion = UpdateDepartmentInputModel.RowVersion,
                    InstructorID = UpdateDepartmentInputModel.InstructorID
                });
                MudDialog.Close(DialogResult.Ok(true));
            }
            catch (ApiException ex)
            {
                var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(ex.Response);

                if (problemDetails != null)
                {
                    _customValidation.DisplayErrors(problemDetails.Errors);
                }
            }
            catch (Exception)
            {
                ErrorVisible = true;
            }
        }
    }

    public void Cancel()
    {
        MudDialog.Cancel();
    }
}
