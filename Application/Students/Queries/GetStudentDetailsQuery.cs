
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Domain.Entities.Projections.Students;
using Domain.Entities.Projections.Mappers;

namespace Application.Students.Queries;

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

        return StudentProjectionMapper.ToStudentDetailProjection(student);
    }
}