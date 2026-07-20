using System;

namespace WebUI.Client.Dtos.Students;

public record CreateStudentDto(string LastName, string FirstName, DateTime EnrollmentDate, string ProfilePictureName);
