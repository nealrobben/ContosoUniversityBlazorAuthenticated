﻿namespace ContosoUniversityBlazor.Application.Instructors.Commands.UpdateInstructor;

using Application.Common.Interfaces;
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Domain.Entities;
using global::Application.Common.Interfaces;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUI.Shared.Instructors.Commands.UpdateInstructor;

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
            .FirstOrDefaultAsync(m => m.ID == request.InstructorID, cancellationToken);

        if (instructorToUpdate == null)
            throw new NotFoundException(nameof(Instructor), request.InstructorID);

        if(!Equals(instructorToUpdate.ProfilePictureName, request.ProfilePictureName))
            _profilePictureService.DeleteImageFile(instructorToUpdate.ProfilePictureName);

        instructorToUpdate.LastName = request.LastName;
        instructorToUpdate.FirstMidName = request.FirstName;
        instructorToUpdate.HireDate = request.HireDate;
        instructorToUpdate.ProfilePictureName = request.ProfilePictureName;

        if (instructorToUpdate.OfficeAssignment == null)
            instructorToUpdate.OfficeAssignment = new OfficeAssignment();

        instructorToUpdate.OfficeAssignment.Location = request.OfficeLocation;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
