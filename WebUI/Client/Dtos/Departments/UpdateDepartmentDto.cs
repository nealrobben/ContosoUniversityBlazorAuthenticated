using System;

namespace WebUI.Client.Dtos.Departments;

public record UpdateDepartmentDto(int DepartmentId, string Name, decimal Budget, DateTime StartDate, byte[] RowVersion, int InstructorId);
