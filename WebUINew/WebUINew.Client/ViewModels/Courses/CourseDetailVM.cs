namespace WebUINew.Client.ViewModels.Courses;

public class CourseDetailVM
{
    public int CourseId { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentID { get; set; }

    public override string ToString()
    {
        return $"{CourseId} - {Title}";
    }
}