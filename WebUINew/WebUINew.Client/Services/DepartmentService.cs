using System.Net.Http.Json;
using WebUINew.Client.Dtos.Common;
using WebUINew.Client.Dtos.Departments;

namespace WebUINew.Client.Services;

public interface IDepartmentService
    : IServiceBase<OverviewDto<DepartmentOverviewDto>, DepartmentDetailDto,
        CreateDepartmentDto, UpdateDepartmentDto>
{
    Task<DepartmentsLookupDto> GetLookupAsync();
}

public class DepartmentService
    : ServiceBase<OverviewDto<DepartmentOverviewDto>, DepartmentDetailDto,
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