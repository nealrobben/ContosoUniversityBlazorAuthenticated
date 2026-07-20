using System;

namespace WebUI.Client.Dtos.Departments;

public record DepartmentOverviewDto(int DepartmentID, string Name, decimal Budget, DateTime StartDate, string AdministratorName);
