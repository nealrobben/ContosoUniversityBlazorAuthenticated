
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Services;
using WebUI.Client.ViewModels.Departments;

namespace WebUI.Client.Pages.Departments;

public partial class DepartmentDetails
{
    [CascadingParameter] 
    MudDialogInstance MudDialog { get; set; }

    [Inject]
    private IDepartmentService DepartmentService { get; set; }

    [Inject]
    private IStringLocalizer<DepartmentDetails> Localizer { get; set; }

    public DepartmentDetailVM Department { get; set; }

    [Parameter] 
    public int DepartmentId { get; set; }

    protected async override Task OnParametersSetAsync()
    {
        var dto = await DepartmentService.GetAsync(DepartmentId.ToString());

        Department = new DepartmentDetailVM
        {
            DepartmentID = dto.DepartmentID,
            Name = dto.Name,
            Budget = dto.Budget,
            StartDate = dto.StartDate,
            AdministratorName = dto.AdministratorName,
            InstructorID = dto.InstructorID,
            RowVersion = dto.RowVersion
        };
    }

    public void Close()
    {
        MudDialog.Cancel();
    }
}
