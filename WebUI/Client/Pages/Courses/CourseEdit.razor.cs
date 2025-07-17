
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Courses;
using WebUI.Client.Extensions;
using WebUI.Client.InputModels.Courses;
using WebUI.Client.Mappers;
using WebUI.Client.Services;
using WebUI.Client.Shared;
using WebUI.Client.ViewModels.Departments;

namespace WebUI.Client.Pages.Courses;

public partial class CourseEdit
{
    [Parameter]
    public int CourseId { get; set; }

    [Inject]
    public ICourseService CourseService { get; set; }

    [Inject]
    public IDepartmentService DepartmentService { get; set; }

    [Inject]
    IStringLocalizer<CourseEdit> Localizer { get; set; }

    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; }

    private CustomValidation _customValidation;

    public bool ErrorVisible { get; set; }

    public UpdateCourseInputModel UpdateCourseInputModel { get; set; } = new UpdateCourseInputModel();
    public DepartmentsLookupVM DepartmentsLookup { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var departmentsLookup = await DepartmentService.GetLookupAsync();
        DepartmentsLookup = DepartmentViewModelMapper.ToViewModel(departmentsLookup);
    }

    protected override async Task OnParametersSetAsync()
    {
        var course = await CourseService.GetAsync(CourseId.ToString());

        UpdateCourseInputModel.CourseID = course.CourseID;
        UpdateCourseInputModel.Credits = course.Credits;
        UpdateCourseInputModel.DepartmentID = course.DepartmentID;
        UpdateCourseInputModel.Title = course.Title;
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
                await CourseService.UpdateAsync(new UpdateCourseDto
                {
                    CourseID = UpdateCourseInputModel.CourseID,
                    Title = UpdateCourseInputModel.Title,
                    Credits = UpdateCourseInputModel.Credits,
                    DepartmentID = UpdateCourseInputModel.DepartmentID
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
