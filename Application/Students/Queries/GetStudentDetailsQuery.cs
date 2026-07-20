using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Projections.Mappers;
using Domain.Entities.Projections.Students;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries;

public class GetStudentDetailsQuery : IRequest<StudentDetail>
{
    public int? Id { get; set; }

    public GetStudentDetailsQuery(int? id)
    {
        Id = id;
    }
}

internal class GetStudentDetailsQueryHandler : IRequestHandler<GetStudentDetailsQuery, StudentDetail>
{
    private readonly ISchoolContext _context;

    public GetStudentDetailsQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<StudentDetail> Handle(GetStudentDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == null)
            throw new NotFoundException(nameof(Student), request.Id);

        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Student), request.Id);

        return StudentProjectionMapper.ToStudentDetailProjection(student);
    }
}