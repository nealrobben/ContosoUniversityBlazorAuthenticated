
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Services;
using WebUI.Shared.Students.Commands.UpdateStudent;

namespace WebUI.Client.Pages.Students;

public partial class StudentEdit
{
    [Inject]
    public IFileuploadService FileuploadService { get; set; }

    [Inject]
    public IStringLocalizer<StudentEdit> Localizer { get; set; }

    [Inject]
    public IStudentService StudentService { get; set; }

    [Parameter]
    public int StudentId { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    public UpdateStudentCommand UpdateStudentCommand { get; set; } = new UpdateStudentCommand();

    public bool ErrorVisible { get; set; }

    public IBrowserFile File { get; set; } = null;

    protected override async Task OnParametersSetAsync()
    {
        var student = await StudentService.GetAsync(StudentId.ToString());

        UpdateStudentCommand.StudentID = student.StudentID;
        UpdateStudentCommand.FirstName = student.FirstName;
        UpdateStudentCommand.LastName = student.LastName;
        UpdateStudentCommand.EnrollmentDate = student.EnrollmentDate;
        UpdateStudentCommand.ProfilePictureName = student.ProfilePictureName;
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
                    UpdateStudentCommand.ProfilePictureName = await FileuploadService.UploadFile(File);
                }

                await StudentService.UpdateAsync(UpdateStudentCommand);
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
