using System.Linq;
using System.Threading.Tasks;
using Application.Departments.Commands;
using Application.Departments.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Departments;
using WebUI.Server.Mappers;

namespace WebUI.Server.Controllers;

public class DepartmentsController : ContosoApiController
{
    [HttpGet]
    public async Task<ActionResult<OverviewDto<DepartmentOverviewDto>>> GetAll(string sortOrder, string searchString, int? pageNumber, int? pageSize)
    {
        var overview = await Mediator.Send(new GetDepartmentsOverviewQuery(sortOrder, searchString, pageNumber, pageSize));

        return Ok(new OverviewDto<DepartmentOverviewDto>
        {
            MetaData = MetaDataDtoMapper.ToDto(overview.MetaData),
            Records = overview.Records.Select(DepartmentDtoMapper.ToDto).ToList()
        });
    }

    [HttpGet("{id}", Name = "GetDepartment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentDetailDto>> Get(string id)
    {
        var department = await Mediator.Send(new GetDepartmentDetailsQuery(int.Parse(id)));

        return Ok(DepartmentDtoMapper.ToDto(department));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
    {
        var departmentID = await Mediator.Send(new CreateDepartmentCommand
        {
            Name = dto.Name,
            Budget = dto.Budget,
            StartDate = dto.StartDate,
            InstructorID = dto.InstructorID
        });
        var result = await Mediator.Send(new GetDepartmentDetailsQuery(departmentID));

        return CreatedAtRoute("GetDepartment", new { id = departmentID.ToString() }, result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteDepartmentCommand(int.Parse(id)));

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update([FromBody] UpdateDepartmentDto dto)
    {
        await Mediator.Send(new UpdateDepartmentCommand
        {
            DepartmentID = dto.DepartmentID,
            Name = dto.Name,
            Budget = dto.Budget,
            StartDate = dto.StartDate,
            RowVersion = dto.RowVersion,
            InstructorID = dto.InstructorID
        });

        return NoContent();
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<DepartmentsLookupDto>> GetLookup()
    {
        var lookup = await Mediator.Send(new GetDepartmentsLookupQuery());

        return Ok(DepartmentDtoMapper.ToDto(lookup));
    }
}
