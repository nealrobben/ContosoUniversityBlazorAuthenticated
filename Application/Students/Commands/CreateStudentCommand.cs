
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Students.Commands;

public class CreateStudentCommand : IRequest<int>
{
    public string LastName { get; set; }

    public string FirstName { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public string ProfilePictureName { get; set; }
}

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, int>
{
    private readonly ISchoolContext _context;

    public CreateStudentCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var newStudent = new Student
        {
            FirstMidName = request.FirstName,
            LastName = request.LastName,
            EnrollmentDate = request.EnrollmentDate,
            ProfilePictureName = request.ProfilePictureName
        };

        _context.Students.Add(newStudent);

        await _context.SaveChangesAsync(cancellationToken);

        return newStudent.ID;
    }
}