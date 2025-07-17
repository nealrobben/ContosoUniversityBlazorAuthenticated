
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Mappers;
using WebUI.Client.Services;
using WebUI.Client.ViewModels.Students;

namespace WebUI.Client.Pages.Students;

public partial class StudentDetails
{
    [Inject]
    public IStudentService StudentService { get; set; }

    [Inject]
    IStringLocalizer<StudentDetails> Localizer { get; set; }

    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; }

    [Parameter]
    public int StudentId { get; set; }

    public StudentDetailVM Student { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var student = await StudentService.GetAsync(StudentId.ToString());
        Student = StudentViewModelMapper.ToViewModel(student);
    }

    public void Close()
    {
        MudDialog.Cancel();
    }
}
