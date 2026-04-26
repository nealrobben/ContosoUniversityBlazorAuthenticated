using WebUINew.Client.Enums;

namespace WebUINew.Client.ViewModels.Students;

public class StudentForCourseVM
{
    public string StudentName { get; set; }
    public Grade? StudentGrade { get; set; }

    public override string ToString()
    {
        return $"{StudentName} - {StudentGrade}";
    }
}