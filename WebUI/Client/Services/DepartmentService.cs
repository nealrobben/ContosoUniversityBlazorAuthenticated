﻿
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Departments;

namespace WebUI.Client.Services;

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
