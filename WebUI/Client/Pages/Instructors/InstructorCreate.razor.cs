
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Instructors;
using WebUI.Client.InputModels.Instructors;
using WebUI.Client.Services;

namespace WebUI.Client.Pages.Instructors;

public partial class InstructorCreate
{
    [Inject]
    public IStringLocalizer<InstructorCreate> Localizer { get; set; }

    [Inject]
    public IFileUploadService FileUploadService { get; set; }

    [Inject]
    public IInstructorService InstructorService { get; set; }

    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; }

    public bool ErrorVisible { get; set; }

    public IBrowserFile File { get; set; }
    public CreateInstructorInputModel CreateInstructorInputModel { get; set; } = new() { HireDate = DateTime.Now.Date };

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
                    CreateInstructorInputModel.ProfilePictureName = await FileUploadService.UploadFile(File);
                }

                await InstructorService.CreateAsync(new CreateInstructorDto
                {
                    FirstName = CreateInstructorInputModel.FirstName,
                    LastName = CreateInstructorInputModel.LastName,
                    HireDate = CreateInstructorInputModel.HireDate,
                    ProfilePictureName = CreateInstructorInputModel.ProfilePictureName
                });

                CreateInstructorInputModel = new CreateInstructorInputModel();
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
