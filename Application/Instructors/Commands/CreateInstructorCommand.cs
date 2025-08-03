using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Instructors.Commands;

public class CreateInstructorCommand : IRequest<int>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime HireDate { get; set; }

    public string ProfilePictureName { get; set; }
}

public class CreateInstructorCommandHandler : IRequestHandler<CreateInstructorCommand, int>
{
    private readonly ISchoolContext _context;

    public CreateInstructorCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
    {
        var newInstructor = new Instructor
        {
            FirstMidName = request.FirstName,
            LastName = request.LastName,
            HireDate = request.HireDate,
            ProfilePictureName = request.ProfilePictureName
        };
        _context.Instructors.Add(newInstructor);

        await _context.SaveChangesAsync(cancellationToken);

        return newInstructor.ID;
    }
}