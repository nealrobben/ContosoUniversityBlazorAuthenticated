
using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using WebUI.Shared.Instructors.Queries.GetInstructorsLookup;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoUniversityBlazor.Application.Instructors.Queries.GetInstructorsLookup;

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
