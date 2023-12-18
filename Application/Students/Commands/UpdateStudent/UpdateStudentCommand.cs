
using Application.Common.Interfaces;
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using System;

namespace ContosoUniversityBlazor.Application.Students.Commands.UpdateStudent;

public class UpdateStudentCommand : IRequest
{
    public int? StudentID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }
}

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
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
        if (request.StudentID == null)
            throw new NotFoundException(nameof(Student), request.StudentID);

        var studentToUpdate = await _context.Students
            .FirstOrDefaultAsync(s => s.ID == request.StudentID, cancellationToken);

        if (!Equals(studentToUpdate.ProfilePictureName, request.ProfilePictureName))
            _profilePictureService.DeleteImageFile(studentToUpdate.ProfilePictureName);

        studentToUpdate.FirstMidName = request.FirstName;
        studentToUpdate.LastName = request.LastName;
        studentToUpdate.EnrollmentDate = request.EnrollmentDate;
        studentToUpdate.ProfilePictureName = request.ProfilePictureName;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
