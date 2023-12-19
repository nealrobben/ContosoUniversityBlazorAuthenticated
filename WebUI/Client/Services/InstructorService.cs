
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Instructors;
using WebUI.Shared.Common;
using WebUI.Shared.Instructors.Queries.GetInstructorsLookup;
using WebUI.Shared.Instructors.Queries.GetInstructorsOverview;

namespace WebUI.Client.Services;

public interface IInstructorService 
    : IServiceBase<OverviewVM<InstructorVM>, InstructorDetailDto,
        CreateInstructorDto, UpdateInstructorDto>
{
    Task<InstructorsLookupVM> GetLookupAsync();
}

public class InstructorService 
    : ServiceBase<OverviewVM<InstructorVM>, InstructorDetailDto,
        CreateInstructorDto, UpdateInstructorDto>, IInstructorService
{
    public InstructorService(HttpClient http) 
        : base(http, "instructors")
    {
    }

    public async Task<InstructorsLookupVM> GetLookupAsync()
    {
        return await _http.GetFromJsonAsync<InstructorsLookupVM>($"{Endpoint}/lookup");
    }
}
