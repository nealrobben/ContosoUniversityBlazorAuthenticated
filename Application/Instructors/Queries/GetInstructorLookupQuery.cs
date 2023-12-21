
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Projections.Instructors;
using Domain.Entities.Projections.Mappers;

namespace ContosoUniversityBlazor.Application.Instructors.Queries.GetInstructorsLookup;

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
