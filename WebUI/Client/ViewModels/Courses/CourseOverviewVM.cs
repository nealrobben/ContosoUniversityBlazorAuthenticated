namespace WebUI.Client.ViewModels.Courses;

public class CourseOverviewVM
{
    public int CourseId { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public string DepartmentName { get; set; }

    public override string ToString()
    {
        return $"{CourseId} - {Title} - {DepartmentName}";
    }
}