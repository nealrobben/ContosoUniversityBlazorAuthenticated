
using ContosoUniversityBlazor.Application.Instructors.Commands.CreateInstructor;
using ContosoUniversityBlazor.Application.Instructors.Commands.DeleteInstructor;
using ContosoUniversityBlazor.Application.Instructors.Commands.UpdateInstructor;
using ContosoUniversityBlazor.Application.Instructors.Queries.GetInstructorDetails;
using ContosoUniversityBlazor.Application.Instructors.Queries.GetInstructorsLookup;
using ContosoUniversityCQRS.Application.Instructors.Queries.GetInstructorsOverview;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Instructors;
using WebUI.Shared.Common;
using WebUI.Shared.Instructors.Queries.GetInstructorsOverview;

namespace ContosoUniversityBlazor.WebUI.Controllers;
public class InstructorsController : ContosoApiController
{
    [HttpGet]
    public async Task<ActionResult<OverviewVM<InstructorVM>>> GetAll(string sortOrder, string searchString, int? pageNumber, int? pageSize)
    {
        var vm = await Mediator.Send(new GetInstructorsOverviewQuery(sortOrder, searchString, pageNumber, pageSize));

        return Ok(vm);
    }

    [HttpGet("{id}", Name = "GetInstructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InstructorDetailDto>> Get(string id)
    {
        var instructor = await Mediator.Send(new GetInstructorDetailsQuery(int.Parse(id)));

        return Ok(new InstructorDetailDto
        {
            InstructorID = instructor.InstructorID,
            LastName = instructor.LastName,
            FirstName = instructor.FirstName,
            HireDate = instructor.HireDate,
            OfficeLocation = instructor.OfficeLocation,
            ProfilePictureName = instructor.ProfilePictureName,
        });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateInstructorDto dto)
    {
        var instructorId = await Mediator.Send(new CreateInstructorCommand
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            HireDate = dto.HireDate,
            ProfilePictureName = dto.ProfilePictureName
        });

        var result = await Mediator.Send(new GetInstructorDetailsQuery(instructorId));

        return CreatedAtRoute("GetInstructor", new { id = instructorId.ToString() }, result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteInstructorCommand(int.Parse(id)));

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update([FromBody] UpdateInstructorDto dto)
    {
        await Mediator.Send(new UpdateInstructorCommand
        {
            InstructorID = dto.InstructorID,
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            HireDate = dto.HireDate,
            OfficeLocation = dto.OfficeLocation,
            ProfilePictureName = dto.ProfilePictureName
        });

        return NoContent();
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<InstructorsLookupDto>> GetLookup()
    {
        var lookup = await Mediator.Send(new GetInstructorLookupQuery());

        return Ok(new InstructorsLookupDto
        {
            Instructors = lookup.Instructors.Select(x => new InstructorLookupDto
            {
                ID = x.ID,
                FullName = x.FullName
            }).ToList()
        });
    }
}
