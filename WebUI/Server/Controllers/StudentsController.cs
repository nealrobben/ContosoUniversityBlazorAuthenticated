
using Application.Students.Commands;
using Application.Students.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Students;
using WebUI.Server.Mappers;

namespace ContosoUniversityBlazor.WebUI.Controllers;
public class StudentsController : ContosoApiController
{
    [HttpGet]
    public async Task<ActionResult<OverviewDto<StudentOverviewDto>>> GetAll(string sortOrder, 
        string searchString, int? pageNumber, int? pageSize)
    {
        var overview = await Mediator.Send(new GetStudentsOverviewQuery(sortOrder, 
            searchString, pageNumber, pageSize));

        return Ok(new OverviewDto<StudentOverviewDto>
        {
            MetaData = MetaDataDtoMapper.ToDto(overview.MetaData),
            Records = overview.Records.Select(StudentDtoMapper.ToDto).ToList()
        });
    }

    [HttpGet("{id}", Name = "GetStudent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentDetailDto>> Get(string id)
    {
        var student = await Mediator.Send(new GetStudentDetailsQuery(int.Parse(id)));

        return Ok(StudentDtoMapper.ToDto(student));
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
    public async Task<ActionResult<StudentsForCourseDto>> ByCourse(string id)
    {
        var studentsForCourse = await Mediator.Send(new GetStudentsForCourseQuery(int.Parse(id)));

        return Ok(StudentDtoMapper.ToDto(studentsForCourse));
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
