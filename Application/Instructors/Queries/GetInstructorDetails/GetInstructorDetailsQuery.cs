namespace ContosoUniversityBlazor.Application.Instructors.Queries.GetInstructorDetails;

using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using WebUI.Shared.Instructors.Queries.GetInstructorDetails;
using Microsoft.EntityFrameworkCore;
using global::System.Threading.Tasks;
using global::System.Threading;

public class GetInstructorDetailsQuery : IRequest<InstructorDetailsVM>
{
    public int? ID { get; set; }

    public GetInstructorDetailsQuery(int? id)
    {
        ID = id;
    }
}

public class GetInstructorDetailsQueryHandler : IRequestHandler<GetInstructorDetailsQuery, InstructorDetailsVM>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    public GetInstructorDetailsQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InstructorDetailsVM> Handle(GetInstructorDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            throw new NotFoundException(nameof(Instructor), request.ID);

        var instructor = await _context.Instructors
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == request.ID, cancellationToken) 
            ?? throw new NotFoundException(nameof(Instructor), request.ID);

        return _mapper.Map<InstructorDetailsVM>(instructor);
    }
}
