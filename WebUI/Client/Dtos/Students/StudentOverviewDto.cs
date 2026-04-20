using System;

namespace WebUI.Client.Dtos.Students;

public class StudentOverviewDto
{
    public int StudentId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}