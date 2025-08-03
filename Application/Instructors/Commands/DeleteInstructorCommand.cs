using Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instructors.Commands;

public class DeleteInstructorCommand : IRequest
{
    public int ID { get; set; }

    public DeleteInstructorCommand(int id)
    {
        ID = id;
    }
}

public class DeleteInstructorCommandHandler : IRequestHandler<DeleteInstructorCommand>
{
    private readonly ISchoolContext _context;
    private readonly IProfilePictureService _profilePictureService;

    public DeleteInstructorCommandHandler(ISchoolContext context, IProfilePictureService profilePictureService)
    {
        _context = context;
        _profilePictureService = profilePictureService;
    }

    public async Task<Unit> Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _context.Instructors
            .Include(i => i.CourseAssignments)
            .SingleOrDefaultAsync(i => i.ID == request.ID, cancellationToken)
            ?? throw new NotFoundException(nameof(Instructor), request.ID);

        if (!string.IsNullOrWhiteSpace(instructor.ProfilePictureName))
            _profilePictureService.DeleteImageFile(instructor.ProfilePictureName);

        var departments = await _context.Departments
            .Where(d => d.InstructorID == request.ID)
            .ToListAsync(cancellationToken);
        departments.ForEach(d => d.InstructorID = null);

        _context.Instructors.Remove(instructor);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}