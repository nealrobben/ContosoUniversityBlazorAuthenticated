using System.Linq;
using System.Threading.Tasks;
using Application.Instructors.Commands;
using Application.Instructors.Queries;
using ContosoUniversityBlazor.Application.Instructors.Queries.GetInstructorsLookup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Instructors;
using WebUI.Server.Mappers;

namespace WebUI.Server.Controllers;

public class InstructorsController : ContosoApiController
{
    [HttpGet]
    public async Task<ActionResult<OverviewDto<InstructorOverviewDto>>> GetAll(string sortOrder, string searchString, int? pageNumber, int? pageSize)
    {
        var overview = await Mediator.Send(new GetInstructorsOverviewQuery(sortOrder, searchString, pageNumber, pageSize));

        return Ok(new OverviewDto<InstructorOverviewDto>
        {
            MetaData = MetaDataDtoMapper.ToDto(overview.MetaData),
            Records = overview.Records.Select(InstructorDtoMapper.ToDto).ToList()
        });
    }

    [HttpGet("{id}", Name = "GetInstructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InstructorDetailDto>> Get(string id)
    {
        var instructor = await Mediator.Send(new GetInstructorDetailsQuery(int.Parse(id)));

        return Ok(InstructorDtoMapper.ToDto(instructor));
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

        return Ok(InstructorDtoMapper.ToDto(lookup));
    }
}
