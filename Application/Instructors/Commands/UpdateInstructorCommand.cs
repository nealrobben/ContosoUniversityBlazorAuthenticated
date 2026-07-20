using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instructors.Commands;

public class UpdateInstructorCommand : IRequest
{
    public int? InstructorId { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime HireDate { get; set; }

    public string OfficeLocation { get; set; }

    public string ProfilePictureName { get; set; }
}

internal class UpdateInstructorCommandHandler : IRequestHandler<UpdateInstructorCommand>
{
    private readonly ISchoolContext _context;
    private readonly IProfilePictureService _profilePictureService;

    public UpdateInstructorCommandHandler(ISchoolContext context, IProfilePictureService profilePictureService)
    {
        _context = context;
        _profilePictureService = profilePictureService;
    }

    public async Task<Unit> Handle(UpdateInstructorCommand request, CancellationToken cancellationToken)
    {
        if (request.InstructorId == null)
            throw new NotFoundException(nameof(Instructor), request.InstructorId);

        var instructorToUpdate = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.CourseAssignments)
            .ThenInclude(i => i.Course)
            .FirstOrDefaultAsync(m => m.ID == request.InstructorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Instructor), request.InstructorId);

        if (!Equals(instructorToUpdate.ProfilePictureName, request.ProfilePictureName))
            await _profilePictureService.DeleteImageFile(instructorToUpdate.ProfilePictureName);

        instructorToUpdate.LastName = request.LastName;
        instructorToUpdate.FirstMidName = request.FirstName;
        instructorToUpdate.HireDate = request.HireDate;
        instructorToUpdate.ProfilePictureName = request.ProfilePictureName;

        instructorToUpdate.OfficeAssignment ??= new OfficeAssignment();

        instructorToUpdate.OfficeAssignment.Location = request.OfficeLocation;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
