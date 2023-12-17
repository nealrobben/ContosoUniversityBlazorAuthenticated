
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Services;
using WebUI.Shared.Instructors.Commands.UpdateInstructor;

namespace WebUI.Client.Pages.Instructors;

public partial class InstructorEdit
{
    [Inject]
    public IFileuploadService FileuploadService { get; set; }

    [Inject]
    public IStringLocalizer<InstructorEdit> Localizer { get; set; }

    [Inject]
    public IInstructorService InstructorService { get; set; }

    [Parameter]
    public int InstructorId { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    public bool ErrorVisible { get; set; }

    public IBrowserFile File { get; set; } = null;
    public UpdateInstructorCommand UpdateInstructorCommand { get; set; } = new UpdateInstructorCommand();

    protected override async Task OnParametersSetAsync()
    {
        var instructor = await InstructorService.GetAsync(InstructorId.ToString());

        UpdateInstructorCommand.InstructorID = instructor.InstructorID;
        UpdateInstructorCommand.FirstName = instructor.FirstName;
        UpdateInstructorCommand.LastName = instructor.LastName;
        UpdateInstructorCommand.HireDate = instructor.HireDate;
        UpdateInstructorCommand.OfficeLocation = instructor.OfficeLocation;
        UpdateInstructorCommand.ProfilePictureName = instructor.ProfilePictureName;
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
                    UpdateInstructorCommand.ProfilePictureName = await FileuploadService.UploadFile(File);
                }

                await InstructorService.UpdateAsync(UpdateInstructorCommand);
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
