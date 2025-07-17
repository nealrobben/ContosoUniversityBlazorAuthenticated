
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Courses;
using WebUI.Client.Extensions;
using WebUI.Client.InputModels.Courses;
using WebUI.Client.Mappers;
using WebUI.Client.Services;
using WebUI.Client.Shared;
using WebUI.Client.ViewModels.Departments;

namespace WebUI.Client.Pages.Courses;

public partial class CourseCreate
{
    [Inject]
    public IStringLocalizer<CourseCreate> Localizer { get; set; }

    [Inject]
    public IDepartmentService DepartmentService { get; set; }

    [Inject]
    public ICourseService CourseService { get; set; }

    public DepartmentsLookupVM DepartmentsLookup { get; set; }

    public bool ErrorVisible { get; set; }

    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; }

    public CreateCourseInputModel CreateCourseInputModel { get; set; } = new CreateCourseInputModel();

    private CustomValidation _customValidation;

    protected override async Task OnInitializedAsync()
    {
        var departmentsLookup = await DepartmentService.GetLookupAsync();
        DepartmentsLookup = DepartmentViewModelMapper.ToViewModel(departmentsLookup);

        CreateCourseInputModel.DepartmentID = DepartmentsLookup.Departments[0].DepartmentID;
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
                await CourseService.CreateAsync(new CreateCourseDto
                {
                    CourseID = CreateCourseInputModel.CourseID,
                    Title = CreateCourseInputModel.Title,
                    Credits = CreateCourseInputModel.Credits,
                    DepartmentID = CreateCourseInputModel.DepartmentID
                });

                CreateCourseInputModel = new CreateCourseInputModel();
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
            catch (System.Exception)
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
