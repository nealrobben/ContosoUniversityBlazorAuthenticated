namespace ContosoUniversityBlazor.Application.Instructors.Queries.GetInstructorsLookup;

using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using WebUI.Shared.Instructors.Queries.GetInstructorsLookup;
using Microsoft.EntityFrameworkCore;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;

public class GetInstructorLookupQuery : IRequest<InstructorsLookupVM>
{
}

public class GetInstructorLookupQueryHandler : IRequestHandler<GetInstructorLookupQuery, InstructorsLookupVM>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    public GetInstructorLookupQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InstructorsLookupVM> Handle(GetInstructorLookupQuery request, CancellationToken cancellationToken)
    {
        var instructors = await _context.Instructors
            .AsNoTracking()
            .OrderBy(x => x.LastName)
            .ToListAsync(cancellationToken);

        return new InstructorsLookupVM(_mapper.Map<List<InstructorLookupVM>>(instructors));
    }
}
