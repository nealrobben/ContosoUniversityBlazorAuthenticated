namespace ContosoUniversityBlazor.Application.Courses.Commands.DeleteCourse;

using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using global::System.Threading;
using global::System.Threading.Tasks;

public class DeleteCourseCommand : IRequest
{
    public int ID { get; set; }

    public DeleteCourseCommand(int id)
    {
        ID = id;
    }
}

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
{
    private readonly ISchoolContext _context;

    public DeleteCourseCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FindAsync(request.ID, cancellationToken);

        if (course == null)
            throw new NotFoundException(nameof(Course), request.ID);

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
