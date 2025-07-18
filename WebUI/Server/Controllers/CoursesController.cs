using System.Linq;
using System.Threading.Tasks;
using Application.Courses.Commands;
using Application.Courses.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Courses;
using WebUI.Server.Mappers;

namespace WebUI.Server.Controllers;

public class CoursesController : ContosoApiController
{
    [HttpGet]
    public async Task<ActionResult<OverviewDto<CourseOverviewDto>>> GetAll(string sortOrder, string searchString, int? pageNumber, int? pageSize)
    {
        var overview = await Mediator.Send(new GetCoursesOverviewQuery(sortOrder, searchString, pageNumber, pageSize));

        return Ok(new OverviewDto<CourseOverviewDto>
        {
            MetaData = MetaDataDtoMapper.ToDto(overview.MetaData),
            Records = overview.Records.Select(CourseDtoMapper.ToDto).ToList()
        });
    }

    [HttpGet("{id}", Name = "GetCourse")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDetailDto>> Get(string id)
    {
        var course = await Mediator.Send(new GetCourseDetailsQuery(int.Parse(id)));

        return Ok(CourseDtoMapper.ToDto(course));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        var courseID = await Mediator.Send(new CreateCourseCommand
        {
            CourseID = dto.CourseID,
            Title = dto.Title,
            Credits = dto.Credits,
            DepartmentID = dto.DepartmentID,
        });

        var result = await Mediator.Send(new GetCourseDetailsQuery(courseID));

        return CreatedAtRoute("GetCourse", new { id = courseID.ToString() }, result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteCourseCommand(int.Parse(id)));

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update([FromBody] UpdateCourseDto dto)
    {
        await Mediator.Send(new UpdateCourseCommand
        {
            CourseID = dto.CourseID,
            Title = dto.Title,
            Credits = dto.Credits,
            DepartmentID = dto.DepartmentID
        });

        return NoContent();
    }

    [HttpGet("byinstructor/{id}")]
    public async Task<ActionResult<CoursesForInstructorOverviewDto>> ByInstructor(string id)
    {
        var coursesOverview = await Mediator.Send(new GetCoursesForInstructorQuery(int.Parse(id)));

        return Ok(CourseDtoMapper.ToDto(coursesOverview));
    }
}
