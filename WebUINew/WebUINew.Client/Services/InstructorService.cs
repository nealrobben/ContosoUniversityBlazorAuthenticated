using System.Net.Http.Json;
using WebUINew.Client.Dtos.Common;
using WebUINew.Client.Dtos.Instructors;

namespace WebUINew.Client.Services;

public interface IInstructorService
    : IServiceBase<OverviewDto<InstructorOverviewDto>, InstructorDetailDto,
        CreateInstructorDto, UpdateInstructorDto>
{
    Task<InstructorsLookupDto> GetLookupAsync();
}

public class InstructorService
    : ServiceBase<OverviewDto<InstructorOverviewDto>, InstructorDetailDto,
        CreateInstructorDto, UpdateInstructorDto>, IInstructorService
{
    public InstructorService(HttpClient http)
        : base(http, "instructors")
    {
    }

    public async Task<InstructorsLookupDto> GetLookupAsync()
    {
        return await _http.GetFromJsonAsync<InstructorsLookupDto>($"{Endpoint}/lookup");
    }
}