using System;
using System.Collections.Generic;

namespace WebUI.Client.Dtos.Students;

public record StudentDetailDto(int StudentID, string LastName, string FirstName, DateTime EnrollmentDate, string ProfilePictureName, List<StudentDetailEnrollmentDto> Enrollments);
