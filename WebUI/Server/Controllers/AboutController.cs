
using Application.Home.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Home;

namespace ContosoUniversityBlazor.WebUI.Controllers;

public class AboutController : ContosoApiController
{
    [HttpGet]
    public async Task<ActionResult<AboutInfoDto>> GetAboutInfo()
    {
        //To demo the loading indicator
        Thread.Sleep(2000);

        var result = await Mediator.Send(new GetAboutInfoQuery());

        return Ok(new AboutInfoDto
        {
            Items = result.Items.Select(x => new EnrollmentDateGroupDto
            {
                EnrollmentDate = x.EnrollmentDate,
                StudentCount = x.StudentCount
            }).ToList()
        });
    }
}
