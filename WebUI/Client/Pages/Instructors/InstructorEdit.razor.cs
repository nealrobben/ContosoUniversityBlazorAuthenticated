namespace WebUI.Client.Pages.Instructors;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Client.Services;
using WebUI.Shared.Instructors.Commands.UpdateInstructor;

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

    public IList<IBrowserFile> Files { get; set; } = new List<IBrowserFile>();
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
                if (Files.Any())
                {
                    UpdateInstructorCommand.ProfilePictureName = await FileuploadService.UploadFile(Files[0]);
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
        foreach (var file in e.GetMultipleFiles())
        {
            Files.Add(file);
        }
    }
}
