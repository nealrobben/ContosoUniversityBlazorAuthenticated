using System;

namespace WebUI.Client.Dtos.Instructors;

public record UpdateInstructorDto(int? InstructorId, string LastName, string FirstName, DateTime HireDate, string OfficeLocation, string ProfilePictureName);
