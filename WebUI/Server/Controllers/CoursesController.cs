
using ContosoUniversityBlazor.Application.Courses.Commands.DeleteCourse;
using ContosoUniversityBlazor.Application.Courses.Commands.UpdateCourse;
using ContosoUniversityBlazor.Application.Courses.Queries.GetCourseDetails;
using ContosoUniversityBlazor.Application.Courses.Queries.GetCoursesOverview;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Courses;
using WebUI.Shared.Common;
using WebUI.Shared.Courses.Commands.CreateCourse;
using WebUI.Shared.Courses.Queries.GetCoursesForInstructor;
using WebUI.Shared.Courses.Queries.GetCoursesOverview;

namespace ContosoUniversityBlazor.WebUI.Controllers;

public class CoursesController : ContosoApiController
{
    [HttpGet]
    public async Task<ActionResult<OverviewVM<CourseVM>>> GetAll(string sortOrder, string searchString, int? pageNumber, int? pageSize)
    {
        var vm = await Mediator.Send(new GetCoursesOverviewQuery(sortOrder, searchString, pageNumber, pageSize));

        return Ok(vm);
    }

    [HttpGet("{id}", Name = "GetCourse")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDetailDto>> Get(string id)
    {
        var course = await Mediator.Send(new GetCourseDetailsQuery(int.Parse(id)));

        return Ok(new CourseDetailDto
        {
            CourseID = course.CourseID,
            Title = course.Title,
            Credits = course.Credits,
            DepartmentID = course.DepartmentID
        });
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
    public async Task<ActionResult<CoursesForInstructorOverviewVM>> ByInstructor(string id)
    {
        var vm = await Mediator.Send(new GetCoursesForInstructorQuery(int.Parse(id)));

        return Ok(vm);
    }
}
