using ContosoUniversityBlazor.Domain.Enums;

namespace WebUI.Client.ViewModels.Students;

public class StudentForCourseVM
{
    public string StudentName { get; set; }
    public Grade? StudentGrade { get; set; }

    public override string ToString()
    {
        return $"{StudentName} - {StudentGrade}";
    }
}
