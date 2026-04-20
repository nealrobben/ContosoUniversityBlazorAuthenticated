namespace WebUI.Client.Dtos.Courses;

public class UpdateCourseDto
{
    public int? CourseId { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentId { get; set; }
}