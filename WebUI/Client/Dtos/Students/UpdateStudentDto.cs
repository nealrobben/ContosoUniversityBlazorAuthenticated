using System;

namespace WebUI.Client.Dtos.Students;

public record UpdateStudentDto(int? StudentId, string LastName, string FirstName, DateTime EnrollmentDate, string ProfilePictureName);
