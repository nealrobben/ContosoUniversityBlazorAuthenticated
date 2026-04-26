namespace WebUINew.Client.Dtos.Students;

public class StudentsForCourseDto
{
    public IList<StudentForCourseDto> Students { get; set; }

    public StudentsForCourseDto()
    {
        Students = [];
    }

    public StudentsForCourseDto(IList<StudentForCourseDto> students)
    {
        Students = students;
    }
}