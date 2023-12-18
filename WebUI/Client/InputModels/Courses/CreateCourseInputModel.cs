namespace WebUI.Client.InputModels.Courses;

public class CreateCourseInputModel
{
    public int CourseID { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentID { get; set; }
}
