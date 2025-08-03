using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.Projections.Instructors;
using Domain.Entities.Projections.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Instructors.Queries;

public class GetInstructorLookupQuery : IRequest<InstructorsLookup>
{
}

public class GetInstructorLookupQueryHandler : IRequestHandler<GetInstructorLookupQuery, InstructorsLookup>
{
    private readonly ISchoolContext _context;

    public GetInstructorLookupQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<InstructorsLookup> Handle(GetInstructorLookupQuery request, CancellationToken cancellationToken)
    {
        var instructors = await _context.Instructors
            .AsNoTracking()
            .OrderBy(x => x.LastName)
            .ToListAsync(cancellationToken);

        return InstructorProjectionMapper.ToInstructorsLookupProjection(instructors);
    }
}
