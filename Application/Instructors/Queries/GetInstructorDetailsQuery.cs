using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Projections.Instructors;
using Domain.Entities.Projections.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instructors.Queries;

public class GetInstructorDetailsQuery : IRequest<InstructorDetail>
{
    public int? Id { get; set; }

    public GetInstructorDetailsQuery(int? id)
    {
        Id = id;
    }
}

internal class GetInstructorDetailsQueryHandler : IRequestHandler<GetInstructorDetailsQuery, InstructorDetail>
{
    private readonly ISchoolContext _context;

    public GetInstructorDetailsQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<InstructorDetail> Handle(GetInstructorDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == null)
            throw new NotFoundException(nameof(Instructor), request.Id);

        var instructor = await _context.Instructors
            .Include(x => x.OfficeAssignment)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Instructor), request.Id);

        return InstructorProjectionMapper.ToInstructorDetailProjection(instructor);
    }
}
