
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Departments;
using WebUI.Shared.Common;
using WebUI.Shared.Departments.Queries.GetDepartmentsOverview;

namespace WebUI.Client.Services;

public interface IDepartmentService
    : IServiceBase<OverviewVM<DepartmentVM>, DepartmentDetailDto,
        CreateDepartmentDto, UpdateDepartmentDto>
{
    Task<DepartmentsLookupDto> GetLookupAsync();
}

public class DepartmentService
    : ServiceBase<OverviewVM<DepartmentVM>, DepartmentDetailDto,
        CreateDepartmentDto, UpdateDepartmentDto>, IDepartmentService
{
    public DepartmentService(HttpClient http)
        : base(http, "departments")
    {
    }

    public async Task<DepartmentsLookupDto> GetLookupAsync()
    {
        return await _http.GetFromJsonAsync<DepartmentsLookupDto>($"{Endpoint}/lookup");
    }
}
