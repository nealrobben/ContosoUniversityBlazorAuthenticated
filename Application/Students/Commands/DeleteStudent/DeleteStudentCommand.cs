namespace ContosoUniversityBlazor.Application.Students.Commands.DeleteStudent;

using Application.Common.Interfaces;
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Domain.Entities;
using global::Application.Common.Interfaces;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class DeleteStudentCommand : IRequest
{
    public int ID { get; set; }

    public DeleteStudentCommand(int id)
    {
        ID = id;
    }
}

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
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
        var student = await _context.Students.SingleOrDefaultAsync(x => x.ID == request.ID, cancellationToken) 
            ?? throw new NotFoundException(nameof(Student), request.ID);

        if (!string.IsNullOrWhiteSpace(student.ProfilePictureName))
            _profilePictureService.DeleteImageFile(student.ProfilePictureName);

        _context.Students.Remove(student);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}