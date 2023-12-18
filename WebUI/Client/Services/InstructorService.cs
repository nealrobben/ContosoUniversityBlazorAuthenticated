
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Instructors;
using WebUI.Shared.Common;
using WebUI.Shared.Instructors.Commands.UpdateInstructor;
using WebUI.Shared.Instructors.Queries.GetInstructorDetails;
using WebUI.Shared.Instructors.Queries.GetInstructorsLookup;
using WebUI.Shared.Instructors.Queries.GetInstructorsOverview;

namespace WebUI.Client.Services;

public interface IInstructorService 
    : IServiceBase<OverviewVM<InstructorVM>, InstructorDetailsVM,
        CreateInstructorDto, UpdateInstructorCommand>
{
    Task<InstructorsLookupVM> GetLookupAsync();
}

public class InstructorService 
    : ServiceBase<OverviewVM<InstructorVM>, InstructorDetailsVM,
        CreateInstructorDto, UpdateInstructorCommand>, IInstructorService
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
