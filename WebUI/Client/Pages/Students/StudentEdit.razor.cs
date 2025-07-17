
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Students;
using WebUI.Client.InputModels.Students;
using WebUI.Client.Services;

namespace WebUI.Client.Pages.Students;

public partial class StudentEdit
{
    [Inject]
    public IFileUploadService FileUploadService { get; set; }

    [Inject]
    public IStringLocalizer<StudentEdit> Localizer { get; set; }

    [Inject]
    public IStudentService StudentService { get; set; }

    [Parameter]
    public int StudentId { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    public UpdateStudentInputModel UpdateStudentInputModel { get; set; } = new UpdateStudentInputModel();

    public bool ErrorVisible { get; set; }

    public IBrowserFile File { get; set; } = null;

    protected override async Task OnParametersSetAsync()
    {
        var student = await StudentService.GetAsync(StudentId.ToString());

        UpdateStudentInputModel.StudentID = student.StudentID;
        UpdateStudentInputModel.FirstName = student.FirstName;
        UpdateStudentInputModel.LastName = student.LastName;
        UpdateStudentInputModel.EnrollmentDate = student.EnrollmentDate;
        UpdateStudentInputModel.ProfilePictureName = student.ProfilePictureName;
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
                    UpdateStudentInputModel.ProfilePictureName = await FileUploadService.UploadFile(File);
                }

                await StudentService.UpdateAsync(new UpdateStudentDto
                {
                    StudentID = UpdateStudentInputModel.StudentID,
                    LastName = UpdateStudentInputModel.LastName,
                    FirstName = UpdateStudentInputModel.FirstName,
                    EnrollmentDate = UpdateStudentInputModel.EnrollmentDate,
                    ProfilePictureName = UpdateStudentInputModel.ProfilePictureName
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
