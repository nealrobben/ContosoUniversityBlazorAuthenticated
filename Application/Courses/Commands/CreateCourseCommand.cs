using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Courses.Commands;

public class CreateCourseCommand : IRequest<int>
{
    public int CourseId { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentId { get; set; }
}

internal class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, int>
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
            CourseID = request.CourseId,
            Title = request.Title,
            Credits = request.Credits,
            DepartmentID = request.DepartmentId
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync(cancellationToken);

        return course.CourseID;
    }
}