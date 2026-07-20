using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands;

public class DeleteStudentCommand : IRequest
{
    public int Id { get; set; }

    public DeleteStudentCommand(int id)
    {
        Id = id;
    }
}

internal class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
{
    private readonly ISchoolContext _context;
    private readonly IProfilePictureService _profilePictureService;

    public DeleteStudentCommandHandler(ISchoolContext context, IProfilePictureService profilePictureService)
    {
        _context = context;
        _profilePictureService = profilePictureService;
    }

    public async Task<Unit> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students.SingleOrDefaultAsync(x => x.ID == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Student), request.Id);

        if (!string.IsNullOrWhiteSpace(student.ProfilePictureName))
            await _profilePictureService.DeleteImageFile(student.ProfilePictureName);

        _context.Students.Remove(student);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}