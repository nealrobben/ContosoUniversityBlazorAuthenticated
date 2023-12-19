using ContosoUniversityBlazor.Domain.Enums;

namespace Domain.Entities.Projections.Students;

public class StudentDetailEnrollment
{
    public string CourseTitle { get; set; }

    public Grade? Grade { get; set; }
}
