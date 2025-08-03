using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Projections.Instructors;
using Domain.Entities.Projections.Mappers;

namespace Application.Instructors.Queries;
public class GetInstructorDetailsQuery : IRequest<InstructorDetail>
{
    public int? ID { get; set; }

    public GetInstructorDetailsQuery(int? id)
    {
        ID = id;
    }
}

public class GetInstructorDetailsQueryHandler : IRequestHandler<GetInstructorDetailsQuery, InstructorDetail>
{
    private readonly ISchoolContext _context;

    public GetInstructorDetailsQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<InstructorDetail> Handle(GetInstructorDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            throw new NotFoundException(nameof(Instructor), request.ID);

        var instructor = await _context.Instructors
            .Include(x => x.OfficeAssignment)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == request.ID, cancellationToken)
            ?? throw new NotFoundException(nameof(Instructor), request.ID);

        return InstructorProjectionMapper.ToInstructorDetailProjection(instructor);
    }
}
