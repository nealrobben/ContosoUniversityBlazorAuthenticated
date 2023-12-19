using ContosoUniversityBlazor.Domain.Enums;

namespace WebUI.Client.Dtos.Students;

public class StudentDetailEnrollmentDto
{
    public string CourseTitle { get; set; }

    public Grade? Grade { get; set; }
}
