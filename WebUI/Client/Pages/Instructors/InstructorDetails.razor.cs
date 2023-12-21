
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Mappers;
using WebUI.Client.Services;
using WebUI.Client.ViewModels.Instructors;

namespace WebUI.Client.Pages.Instructors;

public partial class InstructorDetails
{
    [Inject]
    public IStringLocalizer<InstructorDetails> Localizer { get; set; }

    [Inject]
    public IInstructorService InstructorService { get; set; }

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public int InstructorId { get; set; }

    public InstructorDetailVM Instructor { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var dto = await InstructorService.GetAsync(InstructorId.ToString());

        Instructor = InstructorViewModelMapper.ToViewModel(dto);
    }

    public void Close()
    {
        MudDialog.Cancel();
    }
}
