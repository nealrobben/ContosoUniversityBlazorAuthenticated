
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Instructors;
using WebUI.Client.InputModels.Instructors;
using WebUI.Client.Services;

namespace WebUI.Client.Pages.Instructors;

public partial class InstructorEdit
{
    [Inject]
    public IFileUploadService FileUploadService { get; set; }

    [Inject]
    public IStringLocalizer<InstructorEdit> Localizer { get; set; }

    [Inject]
    public IInstructorService InstructorService { get; set; }

    [Parameter]
    public int InstructorId { get; set; }

    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; }

    public bool ErrorVisible { get; set; }

    public IBrowserFile File { get; set; }
    public UpdateInstructorInputModel UpdateInstructorInputModel { get; set; } = new();

    protected override async Task OnParametersSetAsync()
    {
        var instructor = await InstructorService.GetAsync(InstructorId.ToString());

        UpdateInstructorInputModel.InstructorID = instructor.InstructorID;
        UpdateInstructorInputModel.FirstName = instructor.FirstName;
        UpdateInstructorInputModel.LastName = instructor.LastName;
        UpdateInstructorInputModel.HireDate = instructor.HireDate;
        UpdateInstructorInputModel.OfficeLocation = instructor.OfficeLocation;
        UpdateInstructorInputModel.ProfilePictureName = instructor.ProfilePictureName;
    }

    public async Task FormSubmitted(EditContext editContext)
    {
        bool formIsValid = editContext.Validate();

        if (formIsValid)
        {
            try
            {
                if (File != null)
                {
                    UpdateInstructorInputModel.ProfilePictureName = await FileUploadService.UploadFile(File);
                }

                await InstructorService.UpdateAsync(new UpdateInstructorDto
                {
                    InstructorID = UpdateInstructorInputModel.InstructorID,
                    LastName = UpdateInstructorInputModel.LastName,
                    FirstName = UpdateInstructorInputModel.FirstName,
                    HireDate = UpdateInstructorInputModel.HireDate,
                    OfficeLocation = UpdateInstructorInputModel.OfficeLocation,
                    ProfilePictureName = UpdateInstructorInputModel.ProfilePictureName
                });
                MudDialog.Close(DialogResult.Ok(true));
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

    public void UploadFiles(InputFileChangeEventArgs e)
    {
        File = e.GetMultipleFiles()[0];
    }
}
