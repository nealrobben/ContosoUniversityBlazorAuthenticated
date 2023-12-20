
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Departments;
using System.Linq;
using WebUI.Client.Dtos.Common;
using Application.Departments.Queries;
using Application.Departments.Commands;

namespace ContosoUniversityBlazor.WebUI.Controllers;

public class DepartmentsController : ContosoApiController
{
    [HttpGet]
    public async Task<ActionResult<OverviewDto<DepartmentOverviewDto>>> GetAll(string sortOrder, string searchString, int? pageNumber, int? pageSize)
    {
        var overview = await Mediator.Send(new GetDepartmentsOverviewQuery(sortOrder, searchString, pageNumber, pageSize));

        return Ok(new OverviewDto<DepartmentOverviewDto>
        {
            MetaData = new MetaDataDto
            {
                PageNumber = overview.MetaData.PageNumber,
                TotalPages = overview.MetaData.TotalPages,
                PageSize = overview.MetaData.PageSize,
                TotalRecords = overview.MetaData.TotalRecords,
                CurrentSort = overview.MetaData.CurrentSort,
                SearchString = overview.MetaData.SearchString
            },
            Records = overview.Records.Select(x => new DepartmentOverviewDto
            {
                DepartmentID = x.DepartmentID,
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                AdministratorName = x.AdministratorName
            }).ToList()
        });
    }

    [HttpGet("{id}", Name = "GetDepartment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentDetailDto>> Get(string id)
    {
        var department = await Mediator.Send(new GetDepartmentDetailsQuery(int.Parse(id)));

        return Ok(new DepartmentDetailDto
        {
            DepartmentID = department.DepartmentID,
            Name = department.Name,
            Budget = department.Budget,
            StartDate = department.StartDate,
            AdministratorName = department.AdministratorName,
            InstructorID = department.InstructorID,
            RowVersion = department.RowVersion
        });
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

        return Ok(new DepartmentsLookupDto
        {
            Departments = lookup.Departments.Select(x => new DepartmentLookupDto
            {
                DepartmentID = x.DepartmentID,
                Name = x.Name
            }).ToList()
        });
    }
}
