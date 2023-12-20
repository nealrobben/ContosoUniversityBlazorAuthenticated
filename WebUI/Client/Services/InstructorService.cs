
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Instructors;

namespace WebUI.Client.Services;

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
