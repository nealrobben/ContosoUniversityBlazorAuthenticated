using Domain.Enums;

namespace Domain.Entities.Projections.Students;

public class StudentForCourse
{
    public string StudentName { get; set; }
    public Grade? StudentGrade { get; set; }
}
