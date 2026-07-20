using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Commands;

public class UpdateCourseCommand : IRequest
{
    public int? CourseId { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentId { get; set; }
}

internal class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand>
{
    private readonly ISchoolContext _context;

    public UpdateCourseCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        if (request.CourseId == null)
            throw new NotFoundException(nameof(Course), request.CourseId);

        var courseToUpdate = await _context.Courses
            .FirstOrDefaultAsync(c => c.CourseID == request.CourseId, cancellationToken);

        courseToUpdate.Title = request.Title;
        courseToUpdate.Credits = request.Credits;
        courseToUpdate.DepartmentID = request.DepartmentId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
