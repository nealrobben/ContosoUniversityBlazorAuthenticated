namespace WebUI.Client.Dtos.Courses;

public record UpdateCourseDto(int? CourseId, string Title, int Credits, int DepartmentId);
