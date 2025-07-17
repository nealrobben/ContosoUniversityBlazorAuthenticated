
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Departments;
using WebUI.Client.Extensions;
using WebUI.Client.InputModels.Departments;
using WebUI.Client.Mappers;
using WebUI.Client.Services;
using WebUI.Client.Shared;
using WebUI.Client.ViewModels.Instructors;

namespace WebUI.Client.Pages.Departments;

public partial class DepartmentCreate
{
    [Inject]
    IDepartmentService DepartmentService { get; set; }

    [Inject]
    IInstructorService InstructorService { get; set; }
    
    [Inject]
    IStringLocalizer<DepartmentCreate> Localizer { get; set; }

    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; }

    private CustomValidation _customValidation;

    public CreateDepartmentInputModel CreateDepartmentInputModel { get; set; } = new CreateDepartmentInputModel() { StartDate = DateTime.UtcNow.Date };
    public InstructorsLookupVM InstructorsLookup { get; set; }

    public bool ErrorVisible { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var instructorsLookup = await InstructorService.GetLookupAsync();
        InstructorsLookup = InstructorViewModelMapper.ToViewModel(instructorsLookup);

        CreateDepartmentInputModel.InstructorID = InstructorsLookup.Instructors[0].ID;
        StateHasChanged();
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
                await DepartmentService.CreateAsync(new CreateDepartmentDto
                {
                    Name = CreateDepartmentInputModel.Name,
                    Budget = CreateDepartmentInputModel.Budget,
                    StartDate = CreateDepartmentInputModel.StartDate,
                    InstructorID = CreateDepartmentInputModel.InstructorID
                });

                CreateDepartmentInputModel = new CreateDepartmentInputModel();
                MudDialog.Close(DialogResult.Ok(true));
            }
            catch (ApiException ex)
            {
                var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(ex.Response);

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
