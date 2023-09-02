namespace WebUI.Shared.Courses.Commands.CreateCourse;

using MediatR;

public class CreateCourseCommand : IRequest<int>
{
    public int CourseID { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentID { get; set; }
}
