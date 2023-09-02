namespace WebUI.Shared.Courses.Commands.UpdateCourse;

using MediatR;

public class UpdateCourseCommand : IRequest
{
    public int? CourseID { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentID { get; set; }
}
