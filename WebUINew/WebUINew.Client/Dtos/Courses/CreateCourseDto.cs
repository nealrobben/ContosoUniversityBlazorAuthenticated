namespace WebUINew.Client.Dtos.Courses;

public class CreateCourseDto
{
    public int CourseId { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentId { get; set; }
}