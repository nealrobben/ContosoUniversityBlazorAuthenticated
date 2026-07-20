using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Commands;

public class DeleteCourseCommand : IRequest
{
    public int Id { get; set; }

    public DeleteCourseCommand(int id)
    {
        Id = id;
    }
}

internal class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
{
    private readonly ISchoolContext _context;

    public DeleteCourseCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.SingleOrDefaultAsync(x => x.CourseID == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Course), request.Id);

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
