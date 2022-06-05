﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Shared.Instructors.Commands.CreateInstructor;
using WebUI.Shared.Instructors.Commands.UpdateInstructor;
using WebUI.Shared.Instructors.Queries.GetInstructorDetails;
using WebUI.Shared.Instructors.Queries.GetInstructorsLookup;
using WebUI.Shared.Instructors.Queries.GetInstructorsOverview;

namespace WebUI.Client.Services
{
    public interface IInstructorService 
        : IServiceBase<InstructorsOverviewVM, InstructorDetailsVM,
            CreateInstructorCommand, UpdateInstructorCommand>
    {
        Task<InstructorsLookupVM> GetLookupAsync();
    }

    public class InstructorService 
        : ServiceBase<InstructorsOverviewVM, InstructorDetailsVM, 
            CreateInstructorCommand, UpdateInstructorCommand>, IInstructorService
    {
        protected override string ControllerName => "instructors";

        public InstructorService(HttpClient http) : base(http)
        {
        }

        public async Task<InstructorsLookupVM> GetLookupAsync()
        {
            return await _http.GetFromJsonAsync<InstructorsLookupVM>($"{Endpoint}/lookup");
        }
    }
}
