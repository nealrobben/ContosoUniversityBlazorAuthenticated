using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Courses.Commands;

public class CreateCourseCommand : IRequest<int>
{
    public int CourseID { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentID { get; set; }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, int>
{
    private readonly ISchoolContext _context;

    public CreateCourseCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            CourseID = request.CourseID,
            Title = request.Title,
            Credits = request.Credits,
            DepartmentID = request.DepartmentID
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync(cancellationToken);

        return course.CourseID;
    }
}