
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Students;
using WebUI.Client.InputModels.Students;
using WebUI.Client.Services;

namespace WebUI.Client.Pages.Students;

public partial class StudentCreate
{
    [Inject]
    public IStringLocalizer<StudentCreate> Localizer { get; set; }

    [Inject]
    public IFileUploadService FileUploadService { get; set; }

    [Inject]
    public IStudentService StudentService { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    public CreateStudentInputModel CreateStudentInputModel { get; set; } = new CreateStudentInputModel { EnrollmentDate = DateTime.Now.Date };

    public bool ErrorVisible { get; set; }

    public IBrowserFile File { get; set; } = null;

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
                    CreateStudentInputModel.ProfilePictureName = await FileUploadService.UploadFile(File);
                }

                await StudentService.CreateAsync(new CreateStudentDto
                {
                    FirstName = CreateStudentInputModel.FirstName,
                    LastName = CreateStudentInputModel.LastName,
                    EnrollmentDate = CreateStudentInputModel.EnrollmentDate,
                    ProfilePictureName = CreateStudentInputModel.ProfilePictureName
                });

                CreateStudentInputModel = new CreateStudentInputModel();
                MudDialog.Close(DialogResult.Ok(true));
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

    public void UploadFiles(InputFileChangeEventArgs e)
    {
        File = e.GetMultipleFiles()[0];
    }
}
