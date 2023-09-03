namespace ContosoUniversityBlazor.Application.Courses.Commands.UpdateCourse;

using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUI.Shared.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand>
{
    private readonly ISchoolContext _context;

    public UpdateCourseCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        if (request.CourseID == null)
            throw new NotFoundException(nameof(Course), request.CourseID);

        var courseToUpdate = await _context.Courses
            .FirstOrDefaultAsync(c => c.CourseID == request.CourseID, cancellationToken);

        courseToUpdate.Title = request.Title;
        courseToUpdate.Credits = request.Credits;
        courseToUpdate.DepartmentID = request.DepartmentID;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
