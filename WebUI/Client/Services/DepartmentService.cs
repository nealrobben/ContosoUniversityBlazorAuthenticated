
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Departments;
using WebUI.Shared.Common;
using WebUI.Shared.Departments.Queries.GetDepartmentsLookup;
using WebUI.Shared.Departments.Queries.GetDepartmentsOverview;

namespace WebUI.Client.Services;

public interface IDepartmentService
    : IServiceBase<OverviewVM<DepartmentVM>, DepartmentDetailDto,
        CreateDepartmentDto, UpdateDepartmentDto>
{
    Task<DepartmentsLookupVM> GetLookupAsync();
}

public class DepartmentService
    : ServiceBase<OverviewVM<DepartmentVM>, DepartmentDetailDto,
        CreateDepartmentDto, UpdateDepartmentDto>, IDepartmentService
{
    public DepartmentService(HttpClient http)
        : base(http, "departments")
    {
    }

    public async Task<DepartmentsLookupVM> GetLookupAsync()
    {
        return await _http.GetFromJsonAsync<DepartmentsLookupVM>($"{Endpoint}/lookup");
    }
}
