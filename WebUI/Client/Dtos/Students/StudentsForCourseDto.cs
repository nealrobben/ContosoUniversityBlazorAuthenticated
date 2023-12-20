using System.Collections.Generic;

namespace WebUI.Client.Dtos.Students;

public class StudentsForCourseDto
{
    public IList<StudentForCourseDto> Students { get; set; }

    public StudentsForCourseDto()
    {
        Students = new List<StudentForCourseDto>();
    }

    public StudentsForCourseDto(IList<StudentForCourseDto> students)
    {
        Students = students;
    }
}
