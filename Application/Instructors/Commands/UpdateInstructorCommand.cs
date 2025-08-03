using Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using Application.Common.Exceptions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Instructors.Commands;

public class UpdateInstructorCommand : IRequest
{
    public int? InstructorID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime HireDate { get; set; }

    public string OfficeLocation { get; set; }

    public string ProfilePictureName { get; set; }
}

public class UpdateInstructorCommandHandler : IRequestHandler<UpdateInstructorCommand>
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
        if (request.InstructorID == null)
            throw new NotFoundException(nameof(Instructor), request.InstructorID);

        var instructorToUpdate = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .Include(i => i.CourseAssignments)
            .ThenInclude(i => i.Course)
            .FirstOrDefaultAsync(m => m.ID == request.InstructorID, cancellationToken)
            ?? throw new NotFoundException(nameof(Instructor), request.InstructorID);

        if (!Equals(instructorToUpdate.ProfilePictureName, request.ProfilePictureName))
            _profilePictureService.DeleteImageFile(instructorToUpdate.ProfilePictureName);

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
