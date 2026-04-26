namespace WebUINew.Client.Dtos.Courses;

//TODO: Convert DTOs to Records?
public class CourseDetailDto
{
    public int CourseId { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentId { get; set; }
}