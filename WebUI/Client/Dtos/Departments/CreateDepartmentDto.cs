using System;

namespace WebUI.Client.Dtos.Departments;

public record CreateDepartmentDto(string Name, decimal Budget, DateTime StartDate, int InstructorId);
