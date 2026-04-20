using Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Students.Commands;

public class UpdateStudentCommand : IRequest
{
    public int? StudentId { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }
}

internal class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
{
    private readonly ISchoolContext _context;
    private readonly IProfilePictureService _profilePictureService;

    public UpdateStudentCommandHandler(ISchoolContext context, IProfilePictureService profilePictureService)
    {
        _context = context;
        _profilePictureService = profilePictureService;
    }

    public async Task<Unit> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        if (request.StudentId == null)
            throw new NotFoundException(nameof(Student), request.StudentId);

        var studentToUpdate = await _context.Students
            .FirstOrDefaultAsync(s => s.ID == request.StudentId, cancellationToken);

        if (!Equals(studentToUpdate.ProfilePictureName, request.ProfilePictureName))
            await _profilePictureService.DeleteImageFile(studentToUpdate.ProfilePictureName);

        studentToUpdate.FirstMidName = request.FirstName;
        studentToUpdate.LastName = request.LastName;
        studentToUpdate.EnrollmentDate = request.EnrollmentDate;
        studentToUpdate.ProfilePictureName = request.ProfilePictureName;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
