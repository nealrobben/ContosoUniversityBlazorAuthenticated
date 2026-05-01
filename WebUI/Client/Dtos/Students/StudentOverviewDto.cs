using System;

namespace WebUI.Client.Dtos.Students;

public record StudentOverviewDto(int StudentId, string FirstName, string LastName, DateTime EnrollmentDate)
{
    public string FullName => $"{FirstName} {LastName}";
}