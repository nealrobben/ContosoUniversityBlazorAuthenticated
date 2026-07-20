using System;

namespace WebUI.Client.Dtos.Departments;

public record DepartmentDetailDto(int DepartmentId, string Name, decimal Budget, DateTime StartDate, string AdministratorName, int? InstructorId, byte[] RowVersion);
