
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Threading.Tasks;
using WebUI.Client.Services;
using WebUI.Shared.Instructors.Commands.CreateInstructor;

namespace WebUI.Client.Pages.Instructors;

public partial class InstructorCreate
{
    [Inject]
    public IStringLocalizer<InstructorCreate> Localizer { get; set; }

    [Inject]
    public IFileuploadService FileUploadService { get; set; }

    [Inject]
    public IInstructorService InstructorService { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    public bool ErrorVisible { get; set; }

    public IBrowserFile File { get; set; } = null;
    public CreateInstructorCommand CreateInstructorCommand { get; set; } = new CreateInstructorCommand() { HireDate = DateTime.Now.Date };

    public async Task FormSubmitted(EditContext editContext)
    {
        ErrorVisible = false;
        bool formIsValid = editContext.Validate();

        if (formIsValid)
        {
            try
            {
                if (File != null)
                {
                    CreateInstructorCommand.ProfilePictureName = await FileUploadService.UploadFile(File);
                }

                await InstructorService.CreateAsync(CreateInstructorCommand);

                CreateInstructorCommand = new CreateInstructorCommand();
                MudDialog.Close(DialogResult.Ok(true));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
