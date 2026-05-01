using System;

namespace WebUI.Client.Dtos.Instructors;

public record CreateInstructorDto(string FirstName, string LastName, DateTime HireDate, string ProfilePictureName);
