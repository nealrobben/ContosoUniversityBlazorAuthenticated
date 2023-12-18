﻿
using ContosoUniversityBlazor.Application.Students.Commands.CreateStudent;
using ContosoUniversityBlazor.Application.Students.Commands.DeleteStudent;
using ContosoUniversityBlazor.Application.Students.Commands.UpdateStudent;
using ContosoUniversityBlazor.Application.Students.Queries.GetStudentDetails;
using ContosoUniversityBlazor.Application.Students.Queries.GetStudentsForCourse;
using ContosoUniversityBlazor.Application.Students.Queries.GetStudentsOverview;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Students;
using WebUI.Shared.Common;
using WebUI.Shared.Students.Queries.GetStudentDetails;
using WebUI.Shared.Students.Queries.GetStudentsForCourse;
using WebUI.Shared.Students.Queries.GetStudentsOverview;

namespace ContosoUniversityBlazor.WebUI.Controllers;
public class StudentsController : ContosoApiController
{
    [HttpGet]
    public async Task<ActionResult<OverviewVM<StudentOverviewVM>>> GetAll(string sortOrder, 
        string searchString, int? pageNumber, int? pageSize)
    {
        var vm = await Mediator.Send(new GetStudentsOverviewQuery(sortOrder, 
            searchString, pageNumber, pageSize));

        return Ok(vm);
    }

    [HttpGet("{id}", Name = "GetStudent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentDetailsVM>> Get(string id)
    {
        var vm = await Mediator.Send(new GetStudentDetailsQuery(int.Parse(id)));

        return Ok(vm);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
    {
        var studentId = await Mediator.Send(new CreateStudentCommand
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            EnrollmentDate = dto.EnrollmentDate,
            ProfilePictureName = dto.ProfilePictureName
        });

        var result = await Mediator.Send(new GetStudentDetailsQuery(studentId));

        return CreatedAtRoute("GetStudent", new { id = studentId.ToString() }, result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        await Mediator.Send(new DeleteStudentCommand(int.Parse(id)));

        return NoContent();
    }

    [HttpGet("bycourse/{id}")]
    public async Task<ActionResult<StudentsForCourseVM>> ByCourse(string id)
    {
        var vm = await Mediator.Send(new GetStudentsForCourseQuery(int.Parse(id)));

        return Ok(vm);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update([FromBody] UpdateStudentDto dto)
    {
        await Mediator.Send(new UpdateStudentCommand
        {
            StudentID = dto.StudentID,
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            EnrollmentDate = dto.EnrollmentDate,
            ProfilePictureName = dto.ProfilePictureName
        });

        return NoContent();
    }
}
