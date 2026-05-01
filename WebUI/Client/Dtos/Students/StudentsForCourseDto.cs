using System.Collections.Generic;

namespace WebUI.Client.Dtos.Students;

public record StudentsForCourseDto(IList<StudentForCourseDto> Students)
{
    public StudentsForCourseDto() : this([])
    {
    }
}