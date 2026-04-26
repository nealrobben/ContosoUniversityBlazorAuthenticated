namespace WebUINew.Client.Dtos.Students;

public class UpdateStudentDto
{
    public int? StudentId { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }
}