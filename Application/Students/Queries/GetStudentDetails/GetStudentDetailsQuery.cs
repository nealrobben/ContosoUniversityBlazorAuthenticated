
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Domain.Entities.Projections.Students;
using System.Linq;

namespace ContosoUniversityBlazor.Application.Students.Queries.GetStudentDetails;

public class GetStudentDetailsQuery : IRequest<StudentDetail>
{
    public int? ID { get; set; }

    public GetStudentDetailsQuery(int? id)
    {
        ID = id;
    }
}

public class GetStudentDetailsQueryHandler : IRequestHandler<GetStudentDetailsQuery, StudentDetail>
{
    private readonly ISchoolContext _context;

    public GetStudentDetailsQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<StudentDetail> Handle(GetStudentDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            throw new NotFoundException(nameof(Student), request.ID);

        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == request.ID, cancellationToken)
            ?? throw new NotFoundException(nameof(Student), request.ID);

        return new StudentDetail
        {
            StudentID = student.ID,
            LastName = student.LastName,
            FirstName = student.FirstMidName,
            EnrollmentDate = student.EnrollmentDate,
            ProfilePictureName = student.ProfilePictureName,
            Enrollments = student.Enrollments.Select(x => new StudentDetailEnrollment
            {
                CourseTitle = x.Course.Title,
                Grade = x.Grade,
            }).ToList()
        };
    }
}